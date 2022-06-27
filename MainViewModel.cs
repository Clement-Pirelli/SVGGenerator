using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Drawing;
using System.Windows.Media;

namespace SVGGenerator.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand SaveSVGCommand { get; }
        public ICommand SavePNGCommand { get; }
        public ICommand GenerateCommand { get; }

        private ImageSource _imageSource;
        public ImageSource ImageSource
        {
            get => _imageSource;
            set { _imageSource = value; RaisePropertyChanged(); }
        }

        private int _imageWidth = 500;
        private int _imageHeight = 500;

        public MainViewModel() 
        {
            SaveSVGCommand = new DelegateCommand(SaveSVG, () => _lastSVG is not null);
            SavePNGCommand = new DelegateCommand(SavePNG, () => _lastBMP is not null);
            GenerateCommand = new DelegateCommand(GenerateSVG);
        }

        void RaisePropertyChanged([CallerMemberName] string name = "") 
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private Svg.SvgDocument _lastSVG = null;

        private Bitmap _lastBMP = null;

        internal void OnImageSizeChanged(int width, int height) 
        {
            _imageWidth = width;
            _imageHeight = height;
            GenerateSVG();
        }

        private void GenerateSVG()
        {
            //todo: make this take the size the image control should take
            var imageWidth = _imageWidth;
            var imageHeight = _imageHeight;

            var svgDoc = new Svg.SvgDocument
            {
                Width = imageWidth,
                Height = imageHeight,
                ViewBox = new Svg.SvgViewBox(-imageWidth / 2, -imageHeight / 2, imageWidth, imageHeight),
            };
            var group = new Svg.SvgGroup();
            svgDoc.Children.Add(group);
            group.Children.Add(new Svg.SvgCircle
            {
                Radius = 50,
                Fill = new Svg.SvgColourServer(System.Drawing.Color.Red),
                Stroke = new Svg.SvgColourServer(System.Drawing.Color.Black),
                StrokeWidth = 2
            });

            _lastSVG = svgDoc;
            var result = svgDoc.Draw();
            _lastBMP = result;
            ImageSource = result.ToImageSource();
        }

        private void SaveSVG()
        {
            Console.WriteLine("Saved SVG!");
        }

        private void SavePNG()
        {
            Console.WriteLine("Saved PNG!");
        }
    }
}
