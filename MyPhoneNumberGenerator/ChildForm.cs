using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;
using System.IO;

namespace MyPhoneNumberGenerator
{
    public partial class ChildForm : DevExpress.XtraEditors.XtraForm
    {
        private String _mask;
        private int _len;
        private int[] _permutation;
        private int[] _reverse;
        private int _single = 0;
        private int _multi = 0;
        private int _char = 0;
        private String _format;
        private int _digits = 0;
        private long _count = 1;
        private int _navigatingCountdown = 3;

        public ChildForm(String mask, int len)
        {
            InitializeComponent();
            _mask = mask;
            _len = len;
            _permutation = new int[_mask.Length];
            _reverse = new int[_mask.Length];
            int i, j;
            for (i = _mask.Length; i-- > 0; )
                _permutation[i] = i;

            for (i = _mask.Length; --i > 0; )
                for (j = i; j-- > 0; )
                    if (_mask.Substring(i, 1).CompareTo(_mask.Substring(j, 1)) < 0)
                    {
                        _mask = _mask.Substring(0, j) + _mask.Substring(i, 1) + _mask.Substring(j + 1, i - j - 1) + _mask.Substring(j, 1) + _mask.Substring(i + 1);
                        int k = _permutation[i]; _permutation[i] = _permutation[j]; _permutation[j] = k;
                        Debug.Print(_mask);
                    }
            for (i = _mask.Length; i-- > 0; )
                _reverse[_permutation[i]] = i;

            for (i = _mask.Length; i-- > 0; )
                if (_mask.Substring(i, 1) == "%")
                    _single++;
                else if (_mask.Substring(i, 1) == "*")
                    _multi++;
                else
                    _char++;

            for (i = _len; i > _char; i--)
            {
                _count *= 10;
                _digits++;
            }

            _format = String.Format("D{0}", _digits);

            this.backgroundWorker1.RunWorkerAsync(this);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            ChildForm This = e.Argument as ChildForm;
            int i, j;
            for (i = 0; i < This._mask.Length; i++)
                Debug.Print("{0}", This._permutation[i]);

            while (This._count-- > 0)
            {
                String countString = This._count.ToString(This._format);
                int[] subindexes = new int[This._multi + 1];
                for (i = This._multi; i > 0; i--)
                    subindexes[i] = This._digits;
                subindexes[0] = This._single;
                Boolean doIt = true;

                while (doIt)
                {
                    String[] substrings = new String[This._single + This._multi + This._char];
                    for (i = 0; i < This._single; i++)
                        substrings[i] = countString.Substring(i, 1);
                    for (i = 0; i < This._multi; i++)
                        substrings[This._single + i] = countString.Substring(subindexes[i], subindexes[i + 1] - subindexes[i]);
                    for (i = 0; i < This._char; i++)
                        substrings[This._single + This._multi + i] = This._mask.Substring(This._single + This._multi + i, 1);

                    String resultSpell = "";
                    for (i = 0; i < This._mask.Length; i++)
                        resultSpell += substrings[This._reverse[i]];
                    String resultNumber = Spell2Number(resultSpell);

                    This.AddResult(resultNumber, resultSpell);

                    j = 1;
                    while (subindexes[j] == This._single && j < This._multi) j++;
                    if (j < This._multi)
                    {
                        subindexes[j]--;
                        for (i = 1; i < j; i++)
                            subindexes[i] = subindexes[j];
                    }
                    else
                        doIt = false;

                }

            }
        }

        static private String Spell2Number(String spell)
        {
            String search = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя";
            String replace = "2223334445556667777888999922233344455566677778889999222233333444455556666777788889999222233333444455556666777788889999";
            String number = "";
            for (int i = 0; i < spell.Length; i++)
            {
                int j = search.IndexOf(spell.Substring(i, 1));
                number += (j >= 0) ? replace.Substring(j, 1) : spell.Substring(i, 1);
            }
            return number;
        }

        delegate void AddResultCallback(String number, String spell);

        private void AddResult(String number, String spell)
        {
            if (this.listView1.InvokeRequired)
            {
                AddResultCallback d = new AddResultCallback(AddResult);
                var arr = new object[] { number, spell };
                this.Invoke(d, arr);
            }
            else
            {
                ListViewItem lvi = new ListViewItem(number);
                lvi.SubItems.Add(spell);
                this.listView1.Items.Add(lvi);
            }
        }
        public void SaveAs(String fileName)
        {
            StreamWriter outfile = new StreamWriter(fileName);
            foreach (ListViewItem lvi in this.listView1.Items)
            {
                outfile.WriteLine(lvi.Text);
            }
            outfile.Close();
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (_navigatingCountdown == 0)
            {
                e.Cancel = true;
                SHDocVw.InternetExplorer IE = new SHDocVw.InternetExplorer();
                object Empty = null;
                String URL = e.Url.ToString();
                IE.Visible = true;
                IE.Navigate(URL, ref Empty, ref Empty, ref Empty, ref Empty);
            }
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            _navigatingCountdown--;
        }

    }
}
