﻿#pragma checksum "..\..\..\Pages\Boom.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F5D4C37A2B58A6BA1A8DC0B066687461"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.36373
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Windows.Converters;
using FirstFloor.ModernUI.Windows.Navigation;
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


namespace Peekaboom.Pages {
    
    
    /// <summary>
    /// Boom
    /// </summary>
    public partial class Boom : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 20 "..\..\..\Pages\Boom.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock lguess;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\Pages\Boom.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image image_right;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\Pages\Boom.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image image_left;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\Pages\Boom.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas canvas;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\Pages\Boom.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label wordLabel;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\Pages\Boom.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button b_feed;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\Pages\Boom.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox hintBox;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\Pages\Boom.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button b_sendHint;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\Pages\Boom.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox feedBox;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\Pages\Boom.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label instructionLabel;
        
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
            System.Uri resourceLocater = new System.Uri("/Peekaboom;component/pages/boom.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Pages\Boom.xaml"
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
            this.lguess = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 2:
            this.image_right = ((System.Windows.Controls.Image)(target));
            return;
            case 3:
            this.image_left = ((System.Windows.Controls.Image)(target));
            return;
            case 4:
            this.canvas = ((System.Windows.Controls.Canvas)(target));
            return;
            case 5:
            this.wordLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.b_feed = ((System.Windows.Controls.Button)(target));
            
            #line 38 "..\..\..\Pages\Boom.xaml"
            this.b_feed.Click += new System.Windows.RoutedEventHandler(this.sendFeedback);
            
            #line default
            #line hidden
            return;
            case 7:
            this.hintBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 39 "..\..\..\Pages\Boom.xaml"
            this.hintBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.hintBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 8:
            this.b_sendHint = ((System.Windows.Controls.Button)(target));
            
            #line 40 "..\..\..\Pages\Boom.xaml"
            this.b_sendHint.Click += new System.Windows.RoutedEventHandler(this.b_sendHint_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.feedBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 41 "..\..\..\Pages\Boom.xaml"
            this.feedBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.feedBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 10:
            this.instructionLabel = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

