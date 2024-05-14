using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using GlobalHotKey;
using Clipboard = System.Windows.Clipboard;
using MessageBox = System.Windows.MessageBox;

namespace Language_Swiper
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HotKeyManager hotKeyHideShowManager;
        private HotKeyManager hotKeyChangeManager;
        private string ukrenglower = "qй wц eу rк tе yн uг iш oщ pз [х ]ї aф sі dв fа gп hр jо kл lд ;ж 'є zя xч cс vм bи nт mь ,б .ю";
        private string ukrengUpper = "QЙ WЦ EУ RК TЕ YН UГ IШ OЩ PЗ {Х }Ї AФ SІ DВ FА GП HР JО KЛ LД :Ж \"Є ZЯ XЧ CС VМ BИ NТ MЬ <Б Ю>";

        List<string> ukrenglowerl = new List<string>();
        List<string> ukrengUpperl = new List<string>();
        private bool onoff = true;
        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
        const int VK_SHIFT = 0x10;
        const int VK_CAPITAL = 0x14;
        const int VK_CONTROL = 0x11;
        const int VK_C = 0x43;
        const int VK_V = 0x56;
        const uint KEYEVENTF_KEYUP = 0x0002;
        
        public MainWindow()
        {
            InitializeComponent();
            string[] tempstr;
            tempstr = ukrenglower.Split(' ');
            foreach(var item in tempstr)
            {
                ukrenglowerl.Add(item);
            }
            tempstr = ukrengUpper.Split(' ');
            foreach (var item in tempstr)
            {
                ukrengUpperl.Add(item);
            }
            tempstr = null;
            hotKeyHideShowManager = new HotKeyManager();
            var hotKey = hotKeyHideShowManager.Register(Key.F5, ModifierKeys.Control | ModifierKeys.Alt);
            hotKeyHideShowManager.KeyPressed += HotKeyHideShowManagerPressed;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            cbox.Items.Add("Ukr to Eng");
            cbox.Items.Add("Eng to Urk");
            cbox.SelectedIndex = 0;
            Closing += ClosingProgram;
            
            hotKeyChangeManager = new HotKeyManager();
            var hotKey1 = hotKeyChangeManager.Register(Key.CapsLock, ModifierKeys.Shift );
            hotKeyChangeManager.KeyPressed += HotKeyChangeManagerPressed;
        }

        private async void HotKeyChangeManagerPressed(object sender, KeyPressedEventArgs e)
        {
            //TODO ADD A DATETIME WNEN CLIPBOARD TAKE A FIRST DATA
            string mode = cbox.SelectedItem.ToString();
            string OLDdata = Clipboard.GetText();
            keybd_event(VK_SHIFT, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
            keybd_event(VK_CAPITAL, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
            keybd_event(VK_CONTROL, 0, 0, UIntPtr.Zero);
            keybd_event(VK_C, 0, 0, UIntPtr.Zero);
            keybd_event(VK_C, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
            keybd_event(VK_CONTROL, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
            
            
            await Task.Delay(200);
            string data = Clipboard.GetText();
            
                char[] dataArray = data.ToCharArray();
                string doneData = "";
                if (mode == "Ukr to Eng" || mode == "Eng to Urk")
                {
                    foreach (var item in dataArray)
                    {
                        if (item == ' ')
                        {
                            doneData += ' ';
                        }
                        if (int.TryParse(item.ToString(), out int a))
                        {
                            doneData += item;
                        }
                        if (char.IsUpper(item))
                        {
                            foreach (var itemU in ukrengUpperl)
                            {
                                if (itemU.Contains(item.ToString()))
                                {
                                    doneData += itemU.Remove(itemU.IndexOf(item), 1);
                                    break;
                                }

                            }
                        }
                        else
                        {
                            foreach (var itemU in ukrenglowerl)
                            {
                                if (itemU.Contains(item.ToString()))
                                {
                                    doneData += itemU.Remove(itemU.IndexOf(item), 1);
                                    break;
                                }

                            }
                        }
                    }
                    try
                    {
                        Clipboard.SetText(doneData);
                    }
                    catch
                    {

                    }
                     
                    await Task.Delay(200);
                    keybd_event(VK_CONTROL, 0, 0, UIntPtr.Zero);
                    keybd_event(VK_V, 0, 0, UIntPtr.Zero);
                    keybd_event(VK_V, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
                    keybd_event(VK_CONTROL, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
                }
            

                
            



        }

        private void ClosingProgram(object sender, System.ComponentModel.CancelEventArgs e)
        {
            hotKeyHideShowManager.Dispose();
        }

        private void HotKeyHideShowManagerPressed(object sender, KeyPressedEventArgs e)
        {
            if(onoff)
            {
                Wd.Visibility = Visibility.Visible;
                onoff = false;
            }
            else
            {
                Wd.Visibility = Visibility.Hidden;
                onoff = true;
            }
            
        }
    }
}
