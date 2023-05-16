using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CurrencyToText
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private static Dictionary<char, string> single_numbers = new Dictionary<char, string>()
        {
                {'0', "zero" },
                {'1', "one" },
                {'2', "two" },
                {'3', "three" },
                {'4', "four" },
                {'5', "five" },
                {'6', "six" },
                {'7', "seven" },
                {'8', "eight" },
                {'9', "nine" },

        };

        private static Dictionary<char, string> ten_numbers = new Dictionary<char, string>()
        {
                {'2', "twenty" },
                {'3', "thirty" },
                {'4', "fourty" },
                {'5', "fifty" },
                {'6', "sixty" },
                {'7', "seventy" },
                {'8', "eighty" },
                {'9', "ninety" },
        };

        private Dictionary<string, string> odd_number_names = new Dictionary<string, string>()
        {
                {"10", "ten" },
                {"11", "eleven" },
                {"12", "twelve" },
                {"13", "thirteen" },
                {"14", "fourteen" },
                {"15", "fifteen" },
                {"16", "sixteen" },
                {"17", "seventeen" },
                {"18", "eighteen" },
                {"19", "nineteen" },
        };

        private string[] scales =
        {
            "",
            "thousand ",
            "million "
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            Output.Text = NumbersToText(Input.Text);
        }

        private string NumbersToText(string numbers)
        {
            string final_text;
            string temporary_string = "dollars";
            int scale = 0;
            for (int i = numbers.Length - 1; i >= 0; i--)
            {
                if (i - 2 >= 0 && numbers[i - 2] == ',') // calculate the cents if there are 2 numbers for cents
                {
                    string multiple_cents = "s";

                    if (numbers[i - 1] == '0' && numbers[i] == '1') // instead of showing 'one cents', code get's adjusted to 'one cent'
                    {
                        multiple_cents = "";
                    }

                    temporary_string = temporary_string + " and" + tens(numbers[i - 1], numbers[i]) + "cent" + multiple_cents; // it can also be one dollar or one cent
                    i = i - 2;
                }
                else if (i - 1 >= 0 && numbers[i - 1] == ',') // calculate the cents if there is only 1 number for the cents
                {
                    temporary_string = temporary_string + " and" + tens(numbers[i], '0') + "cents";
                    i = i - 1;
                }
                else // if there are no cents or the cents have already been calculated, go to the converting of the whole numbers
                {
                    temporary_string = scales[scale] + temporary_string; //adds the scales to the word

                    if (i - 2 >= 0) // if there are 3 numbers left
                    {
                        string tens_string = tens(numbers[i - 1], numbers[i]);

                        if (tens_string.Length == 0) // if it's 100, I need a space since my 'tens' function will give back nothing
                        {
                            tens_string = " ";
                        }

                        if (numbers[i - 2] != '0') // Prevent 'one thousand zero hunderd'
                        {
                            temporary_string = single_numbers[numbers[i - 2]] + " hunderd" + tens_string + temporary_string;
                        }
                        else if (numbers[i - 1] != '0')
                        {
                            temporary_string = tens_string.Remove(0, 1) + temporary_string;
                        }

                        i = i - 2;
                        scale += 1;
                    }
                    else if (i - 1 >= 0) // if there are 2 numbers left
                    {
                        temporary_string = tens(numbers[i - 1], numbers[i]) + temporary_string;
                        i = i - 1;
                    }
                    else // if there is only 1 number left
                    {
                        temporary_string = single_numbers[numbers[i]] + " " + temporary_string;
                    }
                }
            }

            if (temporary_string.StartsWith("one dollars")) // replace the 's' of 'dollars' if it is only one dollar
            {
                temporary_string = temporary_string.Remove(10, 1);
            }

            if (temporary_string.StartsWith(" ")) // replace the extra space in front of string. This happens when it is 'twenty dollars'
            {
                temporary_string = temporary_string.Remove(0, 1);
            }

            if (temporary_string.StartsWith("thousand")) // Make sure it 1.000.000 does not show as 'one million thousand dollars'
            {
                temporary_string = temporary_string.Remove(0, 9);
            }

            return temporary_string;
        }

        private string tens(char first_number, char second_number) // 1000 does not work
        {
            string final_string = "";

            if (first_number == '1') // the 10 till 19 names are not logical so I have to use this measurement
            {
                final_string = odd_number_names["" + first_number + second_number];
            }
            else if (first_number != '0')
            {
                if (second_number != '0')
                {
                    final_string = ten_numbers[first_number] + '-' + single_numbers[second_number];
                }
                else
                {
                    final_string = ten_numbers[first_number];
                }
            }
            else if (second_number != '0')
            {
                final_string = single_numbers[second_number];
            }

            if (final_string.Length != 0)
            {
                final_string = " " + final_string + " ";
            }

            return final_string;
        }
    }
}
