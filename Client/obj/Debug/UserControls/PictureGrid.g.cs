﻿#pragma checksum "..\..\..\UserControls\PictureGrid.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "301AAF5E3970AD545433CF6529A3A1E95C8716CA"
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
    /// PictureGrid
    /// </summary>
    public partial class PictureGrid : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 1 "..\..\..\UserControls\PictureGrid.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Client.UserControls.PictureGrid PictureGrid1;
        
        #line default
        #line hidden
        
        
        #line 9 "..\..\..\UserControls\PictureGrid.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid BackGrid;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\..\UserControls\PictureGrid.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox Genres;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\UserControls\PictureGrid.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Search;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\UserControls\PictureGrid.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid PictureView;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\UserControls\PictureGrid.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Prev;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\UserControls\PictureGrid.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Next;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\UserControls\PictureGrid.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label Pages;
        
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
            System.Uri resourceLocater = new System.Uri("/Client;component/usercontrols/picturegrid.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\UserControls\PictureGrid.xaml"
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
            this.PictureGrid1 = ((Client.UserControls.PictureGrid)(target));
            
            #line 8 "..\..\..\UserControls\PictureGrid.xaml"
            this.PictureGrid1.SizeChanged += new System.Windows.SizeChangedEventHandler(this.PictureGrid1_SizeChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.BackGrid = ((System.Windows.Controls.Grid)(target));
            
            #line 9 "..\..\..\UserControls\PictureGrid.xaml"
            this.BackGrid.Loaded += new System.Windows.RoutedEventHandler(this.Grid_Loaded);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 10 "..\..\..\UserControls\PictureGrid.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Genres = ((System.Windows.Controls.ComboBox)(target));
            
            #line 12 "..\..\..\UserControls\PictureGrid.xaml"
            this.Genres.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.Genres_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.Search = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.PictureView = ((System.Windows.Controls.Grid)(target));
            return;
            case 7:
            this.Prev = ((System.Windows.Controls.Button)(target));
            
            #line 19 "..\..\..\UserControls\PictureGrid.xaml"
            this.Prev.Click += new System.Windows.RoutedEventHandler(this.Prev_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.Next = ((System.Windows.Controls.Button)(target));
            
            #line 20 "..\..\..\UserControls\PictureGrid.xaml"
            this.Next.Click += new System.Windows.RoutedEventHandler(this.Next_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.Pages = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

