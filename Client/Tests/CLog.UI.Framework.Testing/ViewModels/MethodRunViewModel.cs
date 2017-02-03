using CLog.Common.Logging;
using CLog.UI.Common.Services;
using CLog.UI.Common.ViewModels;
using CLog.UI.Framework.Testing.Exceptions;
using CLog.UI.Framework.Testing.Models;
using CLog.UI.Framework.Testing.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace CLog.UI.Framework.Testing.ViewModels
{
    public sealed class MethodRunViewModel : ViewModelBase
    {
        private string _description;

        private string _modifier;

        private string _returnType;

        private string _name;

        private string _parameters;

        public MethodRunViewModel(ILogger logger, IStatusService statusService, IDialogService dialogService, IMouseService mouseService, object target, MethodInfo method)
            : base(logger, statusService, dialogService, mouseService)
        {
            Method = method;
            Target = target;

            Modifier = method.IsPublic ? "public" : (method.IsPrivate ? "private" : (method.IsFamily ? "protected" : "internal"));
            ReturnType = method.ReturnType.Name;
            Name = Method.Name;
            Parameters = string.Format("({0})", string.Join(", ", method.GetParameters().Select(p => string.Format("{0} {1}", p.ParameterType.Name, p.Name))));
            Description = string.Format("{0} {1} {2} {3}", Modifier, ReturnType, Name, Parameters);

            RunCommand = CreateCommand(RunCommand_Execute);
        }

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        public string Modifier
        {
            get { return _modifier; }
            set { SetProperty(ref _modifier, value); }
        }

        public string ReturnType
        {
            get { return _returnType; }
            set { SetProperty(ref _returnType, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public string Parameters
        {
            get { return _parameters; }
            set { SetProperty(ref _parameters, value); }
        }

        public MethodInfo Method { get; private set; }

        public object Target { get; private set; }

        public ICommand RunCommand { get; private set; }

        private void RunCommand_Execute(object parameter)
        {
            try
            {
                List<object> parameterValues = new List<object>();
                ParameterInfo[] parameters = Method.GetParameters();
                foreach (ParameterInfo param in parameters)
                {
                    object value = Activator.CreateInstance(param.ParameterType);
                    ValueModel valueModel = null;

                    if (param.ParameterType.FullName.StartsWith("System"))
                    {
                        Type type = typeof(ValueModelGeneric<>);
                        Type generic = type.MakeGenericType(param.ParameterType);
                        valueModel = (ValueModel)Activator.CreateInstance(generic);
                        valueModel.SetValue(value);
                    }
                    else
                    {
                        valueModel = new ValueModelExpandable() { Value = value };
                    }

                    ObjectPropertyGridWindow window = new ObjectPropertyGridWindow(false)
                    {
                        Title = string.Format("Method Parameter:  {0}", param.Name),
                        DataContext = valueModel
                    };

                    if (!window.ShowDialog().Value)
                        throw new CancelException();

                    parameterValues.Add(valueModel.GetValue());
                }

                object result = Method.Invoke(Target, parameterValues.ToArray());
                if (result != null)
                {
                    ObjectPropertyGridWindow resultWindow = new ObjectPropertyGridWindow(true)
                    {
                        Title = string.Format("Return from '{0}'", Name),
                        DataContext = new ValueModelExpandable
                        {
                            Value = result
                        }
                    };

                    resultWindow.Show();
                }

                LoggerHelper.Info(Logger, "Executed method '{0}'", Name);
            }
            catch (CancelException)
            {
                LoggerHelper.Warning(Logger, "Cancelled executing method '{0}'", Name);
            }
            catch (Exception ex)
            {
                LoggerHelper.Exception(Logger, ex, "An error occurred while executing the method '{0}'", Method.Name);
            }
        }
    }
}
