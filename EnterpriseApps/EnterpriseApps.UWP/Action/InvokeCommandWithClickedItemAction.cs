using Microsoft.Xaml.Interactions.Core;
using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace EnterpriseApps.UWP.Action
{
    public class InvokeCommandWithClickedItemAction : DependencyObject, IAction
    {
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(InvokeCommandWithClickedItemAction), new PropertyMetadata(null));

        public object Execute(object sender, object parameter)
        {
            if (Command != null && parameter is ItemClickEventArgs)
                Command.Execute(((ItemClickEventArgs)parameter).ClickedItem);

            return null;
        }
    }
}
