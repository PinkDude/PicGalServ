﻿#pragma checksum "..\..\..\UserControls\Pic.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "81DB87A6AE395F95135F06E6A3E739F374C33641"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using Client.UserControls;
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


namespace Client.UserControls {
    
    
    /// <summary>
    /// Pic
    /// </summary>
    public partial class Pic : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 27 "..\..\..\UserControls\Pic.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image Picture;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\UserControls\Pic.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label Name;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\UserControls\Pic.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label Autor;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\UserControls\Pic.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label Genre;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\UserControls\Pic.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label Date;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\UserControls\Pic.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Id;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\UserControls\Pic.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label Status;
        
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
            System.Uri resourceLocater = new System.Uri("/Client;component/usercontrols/pic.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\UserControls\Pic.xaml"
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
            
            #line 7 "..\..\..\UserControls\Pic.xaml"
            ((Client.UserControls.Pic)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.UserControl_MouseEnter);
            
            #line default
            #line hidden
            
            #line 7 "..\..\..\UserControls\Pic.xaml"
            ((Client.UserControls.Pic)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.UserControl_MouseDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Picture = ((System.Windows.Controls.Image)(target));
            return;
            case 3:
            this.Name = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.Autor = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.Genre = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.Date = ((System.Windows.Controls.Label)(target));
            return;
            case 7:
            this.Id = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            this.Status = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

