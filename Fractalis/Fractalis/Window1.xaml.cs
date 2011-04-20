﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Fractalis.IFS;
using Fractalis.LGrammaire;
using Microsoft.Win32;
using Microsoft.Windows.Controls;
using MessageBox=Microsoft.Windows.Controls.MessageBox;

namespace Fractalis
{
    public partial class Window1
    {
        private readonly FractalisLibrary currentLibrary = new FractalisLibrary();

        /// <summary>
        /// Цвет фрактала. Задается в ColorPicker.
        /// </summary>
        public Color FractalColor
        {
            get { return (Color)GetValue(FractalColorProperty); }
            set { SetValue(FractalColorProperty, value); }
        }
        public static readonly DependencyProperty FractalColorProperty = DependencyProperty.Register(
          "FractalColor", typeof(Color), typeof(Window1),
          new FrameworkPropertyMetadata(Colors.Brown, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                        SelectedColorPropertyChanged));

        #region Constructor

        public Window1()
        {
            InitializeComponent();
            FractalColor = Colors.Black;
            if (!CoordinateTranslator.TestTranslator())
            {
                MessageBox.Show("CoordinateTranslator.TestTranslator failed");
            }
        }

        #endregion

        #region Generate fractal

        private void ButtonFractal_Click(object sender, RoutedEventArgs e)
        {
            string error = TryGenerateFractal();
            if (!String.IsNullOrEmpty(error))
            {
                Microsoft.Windows.Controls.MessageBox.Show(error);
            }
        }

        private string TryGenerateFractal()
        {
            if (String.IsNullOrEmpty(AxiomText.Text))
            {
                return "Не указаны необходимые параметры";
            }

            int depth = String.IsNullOrEmpty(Depth.Text) ? 3 : Convert.ToInt32(Depth.Text);
            double angle = String.IsNullOrEmpty(Angle.Text) ? Math.PI / 3 : Convert.ToDouble(Angle.Text) * Math.PI / 180;
            double beginAngle = String.IsNullOrEmpty(BeginAngle.Text) ? 0 : Convert.ToDouble(BeginAngle.Text) * Math.PI / 180;

            var labirinthusBitmap = new RenderTargetBitmap((int)FractalisImagePanel.ActualWidth,
                (int)FractalisImagePanel.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            var boundRect = new BoundingRectangle
            {
                X0 = 0,
                X1 = FractalisImagePanel.ActualWidth,
                Y0 = 0,
                Y1 = FractalisImagePanel.ActualHeight
            };

            Dictionary<char, string> addRules = LGrammareHelper.ParseAddRules(AddRules.Text);

            string serpWord = LGrammareHelper.TraceInputRules(AxiomText.Text, FRule.Text, bRule.Text, addRules, depth);
            DrawingVisual dw = LGrammareHelper.DrawLFractalis(serpWord, angle, beginAngle, boundRect, 
                new SolidColorBrush(FractalColor));
            labirinthusBitmap.Render(dw);
            FractalisImage.Source = labirinthusBitmap;

            return null;
        }

        #endregion

        #region Validation

        private new void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !LGrammareHelper.LTextAllowed(e.Text);
        }

        private void Value_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                var newText = (String)e.DataObject.GetData(typeof(String));
                if (!LGrammareHelper.LTextAllowed(newText))
                {
                    e.CancelCommand();
                }
            }

            else e.CancelCommand();
        }

        #endregion

        private void LibraryLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var ofd = new OpenFileDialog
                              {
                                  DefaultExt = "*.flb|flb",
                                  Filter = "fractalis lib(*.flb)|*.flb",
                                  FilterIndex = 1,
                                  RestoreDirectory = true,
                                  Title = "Загрузить библиотеку"
                              };

                bool? dr = ofd.ShowDialog();
                if (dr.Value)
                {
                    currentLibrary.LoadLibrary(ofd.FileName);
                    ToolsGrid.DataContext = currentLibrary;
                }
            }
            catch (Exception ex)
            {
                Microsoft.Windows.Controls.MessageBox.Show(ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                currentLibrary.LoadLibrary("..\\..\\..\\Library\\samples.flb");
                ToolsGrid.DataContext = currentLibrary;
            }
            catch (Exception ex)
            {
                Microsoft.Windows.Controls.MessageBox.Show(ex.Message);
            }
        }

        private static void SelectedColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = (Window1)d;
            window.TryGenerateFractal();
        }
    }
}