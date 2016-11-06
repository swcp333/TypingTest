using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
namespace Typing
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>

    public partial class MainWindow : Window
    {
        private int lenofInput = 0;
        private int lenofCheck = 0;
        private string textofCheck = "";
        private string textofInput = "";
        private TextPointer[] CheckTextPointer;


        public MainWindow()
        {
            InitializeComponent();
            string test = "@ 7 ~ ^ + 9 M 2⏎\r\"When you step on the first tee,\" says the four-time golf champion, Bobby Jones, \"you know you can't afford to make one careless slip.\"";
            CheckTextInit(test);
        }


        private void CheckTextInit(string str)
        {
            CheckTextBox.AppendText(str);
            textofCheck = new TextRange(CheckTextBox.Document.ContentStart, CheckTextBox.Document.ContentEnd).Text;
            lenofCheck = textofCheck.Length - 2;
            SaveCheckTextPointer();     //保存节点信息

            GetCheckTextRange(0, Brushes.White, Brushes.BlueViolet);
            for (int i = 1; i < lenofCheck; i++)
            {
                GetCheckTextRange(i, Brushes.Black, Brushes.White);
            }
            PrintKeyboard(0, true);
        }
        private void SaveCheckTextPointer()
        {
            CheckTextPointer = new TextPointer[lenofCheck + 1];
            FlowDocument document = CheckTextBox.Document;
            TextPointer txNext = document.ContentStart;
            for (int i = 0; i <= lenofCheck; i++)
            {
                txNext = txNext.GetNextInsertionPosition(LogicalDirection.Forward);
                CheckTextPointer[i] = txNext;
            }
            for (int i = 0; i < lenofCheck; i++)
            {
                TextRange tr = new TextRange(CheckTextPointer[i], CheckTextPointer[i + 1]);
                //Console.WriteLine(tr.Text);
            }
        }

        private void CheckDifferent(bool insert = true)
        {
            if (insert)
            {
                int nowIndex = lenofInput - 1;
                PrintKeyboard(nowIndex, false); //当前textofCheck[nowIndex]
                if (textofInput[nowIndex] != textofCheck[nowIndex])
                {
                    GetCheckTextRange(nowIndex, Brushes.White, Brushes.OrangeRed);
                    SoundPlayer player = new SoundPlayer(@"./input_wrong.wav");
                    player.Play();
                }
                else
                {
                    GetCheckTextRange(nowIndex, Brushes.DarkGray, Brushes.White);
                }

                if (lenofInput < lenofCheck)        //下一个textofCheck[lenofInput]
                {
                    GetCheckTextRange(lenofInput, Brushes.White, Brushes.BlueViolet);
                    PrintKeyboard(lenofInput, true);
                }

            }
            else
            {
                GetCheckTextRange(lenofInput, Brushes.White, Brushes.BlueViolet);   //上一个
                PrintKeyboard(lenofInput, true);
                int nextIndex = lenofInput + 1;
                if (nextIndex < lenofCheck)
                {
                    GetCheckTextRange(nextIndex, Brushes.Black, Brushes.White);   //当前
                    PrintKeyboard(nextIndex, false);
                }
            }
        }

        private void PrintKeyboard(int Index, bool isDown)
        {
            SolidColorBrush color = isDown ? Brushes.Orange : Brushes.White;
            char chr = textofCheck[Index];

            if ('a' <= chr && chr <= 'z') //小写字母
            {
                ((Label)KeyboardForm.Display.FindName(chr.ToString().ToUpper())).Background = color;
                return;
            }
            if ('0' <= chr && chr <= '9') //数字
            {
                ((Label)KeyboardForm.Display.FindName(string.Concat("D", chr.ToString()))).Background = color;
                return;
            }
            switch (chr)       //其余不需要Shift
            {
                case ' ':
                    ((Label)KeyboardForm.Display.FindName("Space")).Background = color;
                    return;
                case '⏎':
                    ((Label)KeyboardForm.Display.FindName("Return")).Background = color;
                    return;
                case '`':
                    ((Label)KeyboardForm.Display.FindName("Oem3")).Background = color;
                    return;
                case '-':
                    ((Label)KeyboardForm.Display.FindName("OemMinus")).Background = color;
                    return;
                case '=':
                    ((Label)KeyboardForm.Display.FindName("OemPlus")).Background = color;
                    return;
                case '[':
                    ((Label)KeyboardForm.Display.FindName("OemOpenBrackets")).Background = color;
                    return;
                case ']':
                    ((Label)KeyboardForm.Display.FindName("Oem6")).Background = color;
                    return;
                case '\\':
                    ((Label)KeyboardForm.Display.FindName("Oem5")).Background = color;
                    return;
                case ';':
                    ((Label)KeyboardForm.Display.FindName("Oem1")).Background = color;
                    return;
                case '\'':
                    ((Label)KeyboardForm.Display.FindName("OemQuotes")).Background = color;
                    return;
                case ',':
                    ((Label)KeyboardForm.Display.FindName("OemComma")).Background = color;
                    return;
                case '.':
                    ((Label)KeyboardForm.Display.FindName("OemPeriod")).Background = color;
                    return;
                case '/':
                    ((Label)KeyboardForm.Display.FindName("OemQuestion")).Background = color;
                    return;
            }
            int shift = PrintKeyboard_needShift(chr, color);
            if (shift == 1)
            {
                ((Label)KeyboardForm.Display.FindName("RightShift")).Background = color;
                return;
            }
            else if (shift == 2)
            {
                ((Label)KeyboardForm.Display.FindName("LeftShift")).Background = color;
                return;
            }
            return;

        }

        private int PrintKeyboard_needShift(char chr, SolidColorBrush color)
        {
            switch (chr)
            {
                case '~':
                    ((Label)KeyboardForm.Display.FindName("Oem3")).Background = color;       // 右Shift
                    return 1;
                case '!':
                    ((Label)KeyboardForm.Display.FindName("D1")).Background = color;
                    return 1;
                case '@':
                    ((Label)KeyboardForm.Display.FindName("D2")).Background = color;
                    return 1;
                case '#':
                    ((Label)KeyboardForm.Display.FindName("D3")).Background = color;
                    return 1;
                case '$':
                    ((Label)KeyboardForm.Display.FindName("D4")).Background = color;
                    return 1;
                case '%':
                    ((Label)KeyboardForm.Display.FindName("D5")).Background = color;
                    return 1;
                case 'Q':
                    ((Label)KeyboardForm.Display.FindName("Q")).Background = color;
                    return 1;
                case 'W':
                    ((Label)KeyboardForm.Display.FindName("W")).Background = color;
                    return 1;
                case 'E':
                    ((Label)KeyboardForm.Display.FindName("E")).Background = color;
                    return 1;
                case 'R':
                    ((Label)KeyboardForm.Display.FindName("R")).Background = color;
                    return 1;
                case 'T':
                    ((Label)KeyboardForm.Display.FindName("T")).Background = color;
                    return 1;
                case 'A':
                    ((Label)KeyboardForm.Display.FindName("A")).Background = color;
                    return 1;
                case 'S':
                    ((Label)KeyboardForm.Display.FindName("S")).Background = color;
                    return 1;
                case 'D':
                    ((Label)KeyboardForm.Display.FindName("D")).Background = color;
                    return 1;
                case 'F':
                    ((Label)KeyboardForm.Display.FindName("F")).Background = color;
                    return 1;
                case 'G':
                    ((Label)KeyboardForm.Display.FindName("G")).Background = color;
                    return 1;
                case 'Z':
                    ((Label)KeyboardForm.Display.FindName("Z")).Background = color;
                    return 1;
                case 'X':
                    ((Label)KeyboardForm.Display.FindName("X")).Background = color;
                    return 1;
                case 'C':
                    ((Label)KeyboardForm.Display.FindName("C")).Background = color;
                    return 1;
                case 'V':
                    ((Label)KeyboardForm.Display.FindName("V")).Background = color;
                    return 1;
                case 'B':
                    ((Label)KeyboardForm.Display.FindName("B")).Background = color;
                    return 1;
                case '^':
                    ((Label)KeyboardForm.Display.FindName("D6")).Background = color;// 左Shift
                    return 2;
                case '&':
                    ((Label)KeyboardForm.Display.FindName("D7")).Background = color;
                    return 2;
                case '*':
                    ((Label)KeyboardForm.Display.FindName("D8")).Background = color;
                    return 2;
                case '(':
                    ((Label)KeyboardForm.Display.FindName("D9")).Background = color;
                    return 2;
                case ')':
                    ((Label)KeyboardForm.Display.FindName("D0")).Background = color;
                    return 2;
                case '_':
                    ((Label)KeyboardForm.Display.FindName("OemMinus")).Background = color;
                    return 2;
                case '+':
                    ((Label)KeyboardForm.Display.FindName("OemPlus")).Background = color;
                    return 2;
                case 'Y':
                    ((Label)KeyboardForm.Display.FindName("Y")).Background = color;
                    return 2;
                case 'U':
                    ((Label)KeyboardForm.Display.FindName("U")).Background = color;
                    return 2;
                case 'I':
                    ((Label)KeyboardForm.Display.FindName("I")).Background = color;
                    return 2;
                case 'O':
                    ((Label)KeyboardForm.Display.FindName("O")).Background = color;
                    return 2;
                case 'P':
                    ((Label)KeyboardForm.Display.FindName("P")).Background = color;
                    return 2;
                case '{':
                    ((Label)KeyboardForm.Display.FindName("OemOpenBrackets")).Background = color;
                    return 2;
                case '}':
                    ((Label)KeyboardForm.Display.FindName("Oem6")).Background = color;
                    return 2;
                case '|':
                    ((Label)KeyboardForm.Display.FindName("Oem5")).Background = color;
                    return 2;
                case 'H':
                    ((Label)KeyboardForm.Display.FindName("H")).Background = color;
                    return 2;
                case 'J':
                    ((Label)KeyboardForm.Display.FindName("J")).Background = color;
                    return 2;
                case 'K':
                    ((Label)KeyboardForm.Display.FindName("K")).Background = color;
                    return 2;
                case 'L':
                    ((Label)KeyboardForm.Display.FindName("L")).Background = color;
                    return 2;
                case ':':
                    ((Label)KeyboardForm.Display.FindName("Oem1")).Background = color;
                    return 2;
                case '"':
                    ((Label)KeyboardForm.Display.FindName("OemQuotes")).Background = color;
                    return 2;
                case 'N':
                    ((Label)KeyboardForm.Display.FindName("N")).Background = color;
                    return 2;
                case 'M':
                    ((Label)KeyboardForm.Display.FindName("M")).Background = color;
                    return 2;
                case '<':
                    ((Label)KeyboardForm.Display.FindName("OemComma")).Background = color;
                    return 2;
                case '>':
                    ((Label)KeyboardForm.Display.FindName("OemPeriod")).Background = color;
                    return 2;
                case '?':
                    ((Label)KeyboardForm.Display.FindName("OemQuestion")).Background = color;
                    return 2;
            }
            return 0;
        }


        private void GetCheckTextRange(int n, SolidColorBrush Foreground, SolidColorBrush Background)
        {
            TextRange tr = new TextRange(CheckTextPointer[n], CheckTextPointer[n + 1]);
            tr.ApplyPropertyValue(TextElement.ForegroundProperty, Foreground);
            tr.ApplyPropertyValue(TextElement.BackgroundProperty, Background);
        }

        private void KeyboardForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                e.Handled = true;   //无论是否能删都锁定，避免触发PreviewTextInput()
                if (lenofInput > 0)
                {
                    textofInput = textofInput.Substring(0, --lenofInput);
                    CheckDifferent(false);
                    if (textofCheck[lenofInput] == '\r')
                    {
                        textofInput = textofInput.Substring(0, --lenofInput);
                        CheckDifferent(false);
                    }
                }
                return;
            }
            if ((e.Key == Key.Return || e.Key == Key.Enter) && textofCheck[lenofInput] != '⏎')
            {
                e.Handled = true;
                return;
            }
            if (textofCheck[lenofInput] == '⏎')
            {
                e.Handled = true;
                if (e.Key == Key.Return || e.Key == Key.Enter)
                {
                    textofInput = string.Concat(textofInput, "⏎");
                    lenofInput++;
                    CheckDifferent();
                    textofInput = string.Concat(textofInput, "\r");
                    lenofInput++;
                    CheckDifferent();
                    return;
                }
            }
            if (lenofInput >= lenofCheck)
            {
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Escape || e.Key == Key.Tab || e.Key == Key.System) //屏蔽ESC、TAB、ALT
            {
                e.Handled = true;
            }
        }

        private void KeyboardForm_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            textofInput = string.Concat(textofInput, e.Text);
            lenofInput++;
            CheckDifferent();
        }


    }
}
