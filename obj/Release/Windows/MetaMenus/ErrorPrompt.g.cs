﻿#pragma checksum "..\..\..\..\Windows\MetaMenus\ErrorPrompt.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E3DC45AD12730BFBBAAC119250F0484C"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using WpfApp1.Windows.MetaMenus;


namespace WpfApp1.Windows.MetaMenus {
    
    
    /// <summary>
    /// ErrorPrompt
    /// </summary>
    public partial class ErrorPrompt : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 1 "..\..\..\..\Windows\MetaMenus\ErrorPrompt.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal WpfApp1.Windows.MetaMenus.ErrorPrompt ErrorPromptWindow;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\..\Windows\MetaMenus\ErrorPrompt.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ErrorPromptExit;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/TheUndergroundTower;component/windows/metamenus/errorprompt.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Windows\MetaMenus\ErrorPrompt.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.ErrorPromptWindow = ((WpfApp1.Windows.MetaMenus.ErrorPrompt)(target));
            
            #line 10 "..\..\..\..\Windows\MetaMenus\ErrorPrompt.xaml"
            this.ErrorPromptWindow.Closing += new System.ComponentModel.CancelEventHandler(this.ErrorPromptWindow_Closing);
            
            #line default
            #line hidden
            return;
            case 2:
            this.ErrorPromptExit = ((System.Windows.Controls.Button)(target));
            
            #line 13 "..\..\..\..\Windows\MetaMenus\ErrorPrompt.xaml"
            this.ErrorPromptExit.Click += new System.Windows.RoutedEventHandler(this.ErrorPromptExit_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

