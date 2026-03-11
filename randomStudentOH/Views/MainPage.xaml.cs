using randomStudentOH.ViewModels;
using System.Diagnostics;

namespace randomStudentOH.Views
{
    public partial class MainPage : ContentPage
    {
        MainViewModel vm;
        Label[,] pixels;
        public MainPage()
        {
            InitializeComponent();
            int rows = 5;
            int cols = 7;

            pixels = new Label[rows, cols];

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    Label pixel = new Label
                    {
                        HeightRequest = 50,
                        WidthRequest = 50,
                        BackgroundColor = Colors.Black
                    };

                    pixels[y, x] = pixel;

                    Grid.SetRow(pixel, y);
                    Grid.SetColumn(pixel, x);

                    ClockGrid.Children.Add(pixel);
                }
            }
            vm = new MainViewModel();
            BindingContext = vm;

            vm.Animate += AnimatePage;
        }
        void SetPixel(int x, int y, bool active)
        {
            pixels[y, x].BackgroundColor = active ? Color.FromHex("#92DCE5") : Colors.Black;
        }
        async Task DrawNumber(int num)
        {
            for (int y = 0; y < pixels.GetLength(0); y++)
            {
                for (int x = 0; x < pixels.GetLength(1); x++)
                {
                    SetPixel(x, y, false);
                }
            }
            if (num <= 9)
            {
                int[,] numPattern = new int[5, 3];
                for(int i = 0; i <5; i++)
                {
                    for(int j = 0; j < 3; j++)
                    {
                        numPattern[i, j] = numbers[num,i,j];
                    }
                }

                for (int y = 0; y < numPattern.GetLength(0); y++)
                {
                    for (int x = 0; x < numPattern.GetLength(1); x++)
                    {
                        SetPixel(x+2, y, numPattern[y, x] == 1);

                    }
                    await Task.Delay(100);
                }
                return;
            }
            else
            {
                int[,] num1Pattern = new int[5, 3];
                int[,] num2Pattern = new int[5, 3];
                int firstNum = num / 10;
                int secondNum = num - (firstNum*10);
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        num1Pattern[i, j] = numbers[firstNum, i, j];
                        num2Pattern[i, j] = numbers[secondNum, i, j];
                    }
                }
                for (int y = 0; y < num1Pattern.GetLength(0); y++)
                {
                    for (int x = 0; x < num1Pattern.GetLength(1); x++)
                    {
                        SetPixel(x, y, num1Pattern[y, x] == 1);
                        SetPixel(x+4, y, num2Pattern[y, x] == 1);

                    }
                    await Task.Delay(100);
                }
            }

            
        }
        async void AnimatePage(int num)
        {
            
            DrawNumber(num);
        }

        int[,,] numbers = new int[,,]
        {
            {
                {1,1,1},
                {1,0,1},
                {1,0,1},
                {1,0,1},
                {1,1,1}
            },
            {
                {0,1,0},
                {1,1,0},
                {0,1,0},
                {0,1,0},
                {1,1,1}
            },
            {
                {1,1,0},
                {0,0,1},
                {0,1,0},
                {1,0,0},
                {1,1,1}
            },
            {
                {1,1,0},
                {0,0,1},
                {0,1,0},
                {0,0,1},
                {1,1,0}
            },
            {
                {1,0,1},
                {1,0,1},
                {1,1,1},
                {0,0,1},
                {0,0,1}
            },
            {
                {1,1,1},
                {1,0,0},
                {1,1,0},
                {0,0,1},
                {1,1,0}
            },
            {
                {1,1,1},
                {1,0,0},
                {1,1,1},
                {1,0,1},
                {1,1,1}
            },
            {
                {1,1,1},
                {0,0,1},
                {0,1,0},
                {0,1,0},
                {0,1,0}
            },
            {
                {1,1,1},
                {1,0,1},
                {1,1,1},
                {1,0,1},
                {1,1,1}
            },
            {
                {1,1,1},
                {1,0,1},
                {1,1,1},
                {0,0,1},
                {1,1,1}
            }

        };

    }
}

