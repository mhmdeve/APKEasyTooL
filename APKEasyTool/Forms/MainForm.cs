using APKEasyTool.Forms;
using APKEasyTool.Utils;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Media;
using System.Text;
using System.Windows.Forms;

namespace APKEasyTool
{
    public partial class MainForm : Form
    {
        #region fields
        public static MainForm form;
        public static LogOutputForm _logform;

        internal UpdateForm updateForm;
        internal TabLog tabLogInstance;
        internal TabSmali tabSmaliInstance;
        internal TabFramework tabFrameworkInstance;
        internal TabOptions tabOptionsInstance;
        internal TabMain tabMainInstance;
        internal MainClass mainFormInstance;
        internal ApkInfoForm apkInfoForm;
        internal Apktool apkTool;
        internal Lang language;
        internal LogOutputForm logOutputForm;
        internal CMD cmd;

        public bool isLoaded, isSelectedFromDD, LanguageLoaded, dontSave;
        #endregion

        public MainForm()
        {
            InitializeComponent();

            updateForm = new UpdateForm(this);
            tabLogInstance = new TabLog(this);
            tabFrameworkInstance = new TabFramework(this);
            tabSmaliInstance = new TabSmali(this);
            tabOptionsInstance = new TabOptions(this);
            tabMainInstance = new TabMain(this);
            mainFormInstance = new MainClass(this);
            apkInfoForm = new ApkInfoForm(this);
            apkTool = new Apktool(this);
            language = new Lang(this);
            logOutputForm = new LogOutputForm(this);
            cmd = new CMD(this);

            // eventhandler
            #region eventhandler

            //----- Main -----//
            selectApk.Click += new EventHandler((sender, e) => tabMainInstance.selectApk_Click(sender, e));
            logoPanel.Click += new EventHandler((sender, e) => { UpdateForm form = new UpdateForm(); form.Show(); });

            tabMain.DragLeave += new EventHandler((sender, e) => tabMain.BackColor = Color.White);
            tabMain.DragEnter += new DragEventHandler((sender, e) => {
                string[] files = e.GetFilesDrop();
                FileAttributes attr = File.GetAttributes(files[0]);
                if (attr.HasFlag(FileAttributes.Directory))
                    e.CheckDragEnter();
                else
                    e.CheckDragEnter(".apk", ".jar");
            });
            tabMain.DragOver += new DragEventHandler((sender, e) => {
                string[] files = e.GetFilesDrop();
                FileAttributes attr = File.GetAttributes(files[0]);
                if (attr.HasFlag(FileAttributes.Directory))
                    tabMain.BackColor = Color.PowderBlue;
                else if (e.CheckDragOver(".apk", ".jar"))
                    tabMain.BackColor = Color.PowderBlue;
            });

            tabMain.DragDrop += new DragEventHandler((sender, e) => { DragDropUtils.tab1_DragDrop(this, sender, e); });

            decApkBtn.DragLeave += new EventHandler((sender, e) => decApkBtn.BackColor = Color.Transparent);
            decApkBtn.DragEnter += new DragEventHandler((sender, e) => e.CheckDragEnter(".apk", ".jar"));
            decApkBtn.DragOver += new DragEventHandler((sender, e) => { if (e.CheckDragOver(".apk", ".jar")) decApkBtn.BackColor = Color.PowderBlue; });
            decApkBtn.DragDrop += new DragEventHandler((sender, e) => { DragDropUtils.decApkBtn_DragDrop(this, sender, e); });
            decApkBtn.Click += new EventHandler((sender, e) => { tabMainInstance.decApkBtn_Click(sender, e); });

            comApkBtn.DragLeave += new EventHandler((sender, e) => comApkBtn.BackColor = Color.Transparent);
            comApkBtn.DragEnter += new DragEventHandler((sender, e) => e.CheckDragEnter());
            comApkBtn.DragOver += new DragEventHandler((sender, e) => { if (e.CheckDragOver()) comApkBtn.BackColor = Color.PowderBlue; });
            comApkBtn.DragDrop += new DragEventHandler((sender, e) => { DragDropUtils.comApkBtn_DragDrop(this, sender, e); });
            comApkBtn.Click += new EventHandler((sender, e) => { tabMainInstance.comApkBtn_Click(sender, e); });

            signApkBtn.DragLeave += new EventHandler((sender, e) => signApkBtn.BackColor = Color.Transparent);
            signApkBtn.DragEnter += new DragEventHandler((sender, e) => e.CheckDragEnter(".apk"));
            signApkBtn.DragOver += new DragEventHandler((sender, e) => { if (e.CheckDragOver(".apk")) signApkBtn.BackColor = Color.PowderBlue; });
            signApkBtn.DragDrop += new DragEventHandler((sender, e) => { DragDropUtils.signApkBtn_DragDrop(this, sender, e); });
            signApkBtn.Click += new EventHandler((sender, e) => { tabMainInstance.signapk(); });

            zipAlignBtn.DragLeave += new EventHandler((sender, e) => zipAlignBtn.BackColor = Color.Transparent);
            zipAlignBtn.DragEnter += new DragEventHandler((sender, e) => e.CheckDragEnter(".apk"));
            zipAlignBtn.DragOver += new DragEventHandler((sender, e) => { if (e.CheckDragOver(".apk")) zipAlignBtn.BackColor = Color.PowderBlue; });
            zipAlignBtn.DragDrop += new DragEventHandler((sender, e) => { DragDropUtils.zipAlignBtn_DragDrop(this, sender, e); });
            zipAlignBtn.Click += new EventHandler((sender, e) => { tabMainInstance.Zipalign(); });

            chkAliBtn.DragLeave += new EventHandler((sender, e) => chkAliBtn.BackColor = Color.Transparent);
            chkAliBtn.DragEnter += new DragEventHandler((sender, e) => e.CheckDragEnter(".apk"));
            chkAliBtn.DragOver += new DragEventHandler((sender, e) => { if (e.CheckDragOver(".apk")) chkAliBtn.BackColor = Color.PowderBlue; });
            chkAliBtn.DragDrop += new DragEventHandler((sender, e) => { DragDropUtils.chkAliBtn_DragDrop(this, sender, e); });
            chkAliBtn.Click += new EventHandler((sender, e) => { tabMainInstance.chkAliBtn_Click(sender, e); });

            installApkBtn.DragLeave += new EventHandler((sender, e) => installApkBtn.BackColor = Color.Transparent);
            installApkBtn.DragEnter += new DragEventHandler((sender, e) => e.CheckDragEnter(".apk"));
            installApkBtn.DragOver += new DragEventHandler((sender, e) => { if (e.CheckDragOver(".apk")) installApkBtn.BackColor = Color.PowderBlue; });
            installApkBtn.DragDrop += new DragEventHandler((sender, e) => { DragDropUtils.installApkBtn_DragDrop(this, sender, e); });
            installApkBtn.Click += new EventHandler((sender, e) => { tabMainInstance.installApk(); });

            extractApkBtn.DragLeave += new EventHandler((sender, e) => extractApkBtn.BackColor = Color.Transparent);
            extractApkBtn.DragEnter += new DragEventHandler((sender, e) => e.CheckDragEnter(".apk"));
            extractApkBtn.DragOver += new DragEventHandler((sender, e) => { if (e.CheckDragOver(".apk")) extractApkBtn.BackColor = Color.PowderBlue; });
            extractApkBtn.DragDrop += new DragEventHandler((sender, e) => { DragDropUtils.extractApkBtn_DragDrop(this, sender, e); });
            extractApkBtn.Click += new EventHandler((sender, e) => { tabMainInstance.extractApkBtn_Click(sender, e); });

            zipApkBtn.DragLeave += new EventHandler((sender, e) => zipApkBtn.BackColor = Color.Transparent);
            zipApkBtn.DragEnter += new DragEventHandler((sender, e) => e.CheckDragEnter(".apk"));
            zipApkBtn.DragOver += new DragEventHandler((sender, e) => { if (e.CheckDragOver(".apk")) zipApkBtn.BackColor = Color.PowderBlue; });
            zipApkBtn.DragDrop += new DragEventHandler((sender, e) => { DragDropUtils.zipApkBtn_DragDrop(this, sender, e); });
            zipApkBtn.Click += new EventHandler((sender, e) => { tabMainInstance.zipApkBtn_Click(sender, e); });

            installFwBtn.DragLeave += new EventHandler((sender, e) => installFwBtn.BackColor = Color.Transparent);
            installFwBtn.DragEnter += new DragEventHandler((sender, e) => e.CheckDragEnter(".apk"));
            installFwBtn.DragOver += new DragEventHandler((sender, e) => { if (e.CheckDragOver(".apk")) installFwBtn.BackColor = Color.PowderBlue; });
            installFwBtn.DragDrop += new DragEventHandler((sender, e) => { DragDropUtils.installFwBtn_DragDrop(this, sender, e); });
            installFwBtn.Click += new EventHandler((sender, e) => { tabFrameworkInstance.installFwBtn_Click(sender, e); });

            openDecOutput.Click += new EventHandler((sender, e) => { Process.Start("explorer.exe", pathOfDec.Text); });
            openComOutput.Click += new EventHandler((sender, e) => { Process.Start("explorer.exe", pathOfCom.Text); });
            openExt.Click += new EventHandler((sender, e) => { Process.Start("explorer.exe", pathOfExt.Text); });
            openZipApk.Click += new EventHandler((sender, e) => { Process.Start("explorer.exe", pathOfZip.Text); });

            pakLbl.Click += new EventHandler((sender, e) => { tabMainInstance.pakLbl_Click(sender, e); });
            launchLbl.Click += new EventHandler((sender, e) => { tabMainInstance.launchLbl_Click(sender, e); });
            selectDecDir.Click += new EventHandler((sender, e) => { tabMainInstance.selectDecDir_Click(sender, e); });
            fullApkInfoBtn.Click += new EventHandler((sender, e) => { ApkInfoForm form = new ApkInfoForm(); form.Show(); });

            psPicBox.Click += new EventHandler((sender, e) => { if (pakLbl.Text != "---") Process.Start("https://play.google.com/store/apps/details?id=" + pakLbl.Text); });
            acPicBox.Click += new EventHandler((sender, e) => { if (pakLbl.Text != "---") Process.Start("https://apkcombo.com/a/" + pakLbl.Text); });
            apPicBox.Click += new EventHandler((sender, e) => { if (pakLbl.Text != "---") Process.Start("https://apkpure.com/a/" + pakLbl.Text); });

            richTextBoxLogs.LinkClicked += new LinkClickedEventHandler((sender, e) => Process.Start(e.LinkText));

            //----- Smali -----
            tabSmali.DragLeave += new EventHandler((sender, e) => tabSmali.BackColor = Color.White);
            tabSmali.DragEnter += new DragEventHandler((sender, e) => e.CheckDragEnter(".dex", ".odex", ".oat"));
            tabSmali.DragOver += new DragEventHandler((sender, e) => { if (e.CheckDragOver(".dex", ".odex", ".oat")) tabSmali.BackColor = Color.PowderBlue; });
            tabSmali.DragDrop += new DragEventHandler((sender, e) => DragDropUtils.tabPage1_DragDrop(this, sender, e));

            baksSelFileBtn.Click += new EventHandler((sender, e) =>
            {
                DirectoryUtils.selFile(Lang.SEL_DEX_ODEX_DIAG, "Classes.dex (*.dex)|*.dex|Odexed file (*.odex, *.oat)|*.odex;*.oat", file => baksFile.Text = file);
                baksNameTxtBox.Text = Path.GetFileName(baksFile.Text);
            });
            baksChangeBtn.Click += new EventHandler((sender, e) => DirectoryUtils.selFolder(Lang.SEL_DIS_DIR_DIAG, file => baksDir.Text = file));

            smaliDisBtn.DragLeave += new EventHandler((sender, e) => smaliDisBtn.BackColor = Color.Transparent);
            smaliDisBtn.DragEnter += new DragEventHandler((sender, e) => e.CheckDragEnter(".dex", ".odex", ".oat"));
            smaliDisBtn.DragOver += new DragEventHandler((sender, e) => { if (e.CheckDragOver(".dex", ".odex", ".oat")) smaliDisBtn.BackColor = Color.PowderBlue; });
            smaliDisBtn.DragDrop += new DragEventHandler((sender, e) => { DragDropUtils.smaliDisBtn_DragDrop(this, sender, e); });
            smaliDisBtn.Click += new EventHandler((sender, e) => tabSmaliInstance.smaliDisBtn_Click(sender, e));

            smaliSelFileBtn.Click += new EventHandler((sender, e) =>
            {
                DirectoryUtils.selFolder(Lang.SEL_DIS_FOLDER_DIAG, file => smaliFile.Text = file);
                smaliNameTxtBox.Text = Path.GetFileName(smaliFile.Text);
            });
            smaliChangeBtn.Click += new EventHandler((sender, e) => DirectoryUtils.selFolder(Lang.SEL_SMA_DIR_DIAG, file => smaliDir.Text = file));

            smaliAssBtn.DragLeave += new EventHandler((sender, e) => smaliAssBtn.BackColor = Color.Transparent);
            smaliAssBtn.DragEnter += new DragEventHandler((sender, e) => e.CheckDragEnter(null));
            smaliAssBtn.DragOver += new DragEventHandler((sender, e) => { if (e.CheckDragOver(null)) smaliAssBtn.BackColor = Color.PowderBlue; });
            smaliAssBtn.DragDrop += new DragEventHandler((sender, e) => { DragDropUtils.smaliAssBtn_DragDrop(this, sender, e); });
            smaliAssBtn.Click += new EventHandler((sender, e) => tabSmaliInstance.smaliAssBtn_Click(sender, e));

            pakLbl.MouseLeave += new EventHandler((sender, e) => pakLbl.BackColor = Color.WhiteSmoke);
            pakLbl.MouseEnter += new EventHandler((sender, e) => pakLbl.BackColor = Color.LightSkyBlue);
            pakLbl.MouseDown += new MouseEventHandler((sender, e) => pakLbl.BackColor = Color.FromArgb(114, 177, 216));

            launchLbl.MouseLeave += new EventHandler((sender, e) => launchLbl.BackColor = Color.WhiteSmoke);
            launchLbl.MouseEnter += new EventHandler((sender, e) => launchLbl.BackColor = Color.LightSkyBlue);
            launchLbl.MouseDown += new MouseEventHandler((sender, e) => launchLbl.BackColor = Color.FromArgb(114, 177, 216));

            openBaksFolderBtn.Click += new EventHandler((sender, e) => Process.Start("explorer.exe", baksDir.Text));
            openSmaliFolderBtn.Click += new EventHandler((sender, e) => Process.Start("explorer.exe", smaliDir.Text));

            //----- Framework -----
            openFwDirBtn.Click += new EventHandler((sender, e) => tabFrameworkInstance.openFwDirBtn_Click(sender, e));
            selFramework.Click += new EventHandler((sender, e) => DirectoryUtils.selFile(Lang.SEL_FW_DIAG, Lang.AND_FW_PACK_LBL + " (*.apk) |*.apk", file => pathOfFw.Text = file));
            changeInsFwTxtbox.Click += new EventHandler((sender, e) => tabFrameworkInstance.changeInsFwTxtbox_Click(sender, e));
            changeDirFwTxtbox.Click += new EventHandler((sender, e) => tabFrameworkInstance.changeDirFwTxtbox_Click(sender, e));
            clrFwCacheBtn.Click += new EventHandler((sender, e) => tabFrameworkInstance.clrFwCacheBtn_Click(sender, e));

            //----- Option -----
            folderBtn.MouseLeave += new EventHandler((sender, e) => folderBtn.BackColor = Color.Gainsboro);
            folderBtn.MouseEnter += new EventHandler((sender, e) => folderBtn.BackColor = Color.LightSkyBlue);
            folderBtn.Click += new EventHandler((sender, e) => Process.Start("explorer.exe", Variables.RealPath("Apktool")));
            tmpFolBtn.Click += new EventHandler((sender, e) => Process.Start("explorer.exe", Variables.TempPath));

            downBtn.MouseLeave += new EventHandler((sender, e) => downBtn.BackColor = Color.Gainsboro);
            downBtn.MouseEnter += new EventHandler((sender, e) => downBtn.BackColor = Color.LightSkyBlue);

            insBtn.Click += new EventHandler((sender, e) => CMD.ProcessStartWithArgs(Variables.RealPath("ServerRegistrationManager.exe"), "install \"" + Variables.RealPath("AETShellExt.dll") + "\" -codebase"));
            uninsBtn.Click += new EventHandler((sender, e) => { CMD.ProcessStartWithArgs(Variables.RealPath("ServerRegistrationManager.exe"), "uninstall \"" + Variables.RealPath("AETShellExt.dll") + "\""); Explorer.Restart(); });

            setupDirBtn.Click += new EventHandler((sender, e) => tabOptionsInstance.setupDirBtn_Click(sender, e));

            restartBtn.Click += new EventHandler((sender, e) => { Application.Restart(); });

            changeDecDir.Click += new EventHandler((sender, e) => DirectoryUtils.selFolder(Lang.SEL_DEC_DIR_DIAG, file => pathOfDec.Text = file));
            changeComDir.Click += new EventHandler((sender, e) => DirectoryUtils.selFolder(Lang.SEL_COM_DIR_DIAG, file => pathOfCom.Text = file));
            changeExtDir.Click += new EventHandler((sender, e) => DirectoryUtils.selFolder(Lang.SEL_EXT_DIR_DIAG, file => pathOfExt.Text = file));
            changeZipDir.Click += new EventHandler((sender, e) => DirectoryUtils.selFolder(Lang.SEL_ZIP_DIR_DIAG, file => pathOfZip.Text = file));
            selPk8TxtBox.Click += new EventHandler((sender, e) => DirectoryUtils.selFile(Lang.SEL_PK8_FILE_DIAG, ".pk8 key file (*.pk8)|*.pk8", file => pk8FileTxtBox.Text = file));
            selPemTxtBox.Click += new EventHandler((sender, e) => DirectoryUtils.selFile(Lang.SEL_PEM_FILE_DIAG, ".pem key file (*.pem)|*.pem", file => pemFileTxtBox.Text = file));
            selJksTxtBox.Click += new EventHandler((sender, e) => DirectoryUtils.selFile(Lang.SEL_KEY_FILE_DIAG, "Keystore (.jks or .keystore)|*.jks;*.keystore", file => jksFileTxtBox.Text = file));
            setAaptPathBtn.Click += new EventHandler((sender, e) => DirectoryUtils.selFile(Lang.SEL_AAPT_FILE_DIAG, ".exe file (*.exe)|*.exe", file => useAaptPathTxtBox.Text = file));
            selJavaBtn.Click += new EventHandler((sender, e) => DirectoryUtils.selFile(Lang.Localize("sel_java_diag", "Select java.exe file"), "java.exe|java.exe", file => extJavaTxtBox.Text = file));

            //----- Links -----
            downBtn.Click += new EventHandler((sender, e) => Process.Start("https://ibotpeaches.github.io/Apktool/"));
            linkLabelXda.Click += new EventHandler((sender, e) => Process.Start("https://forum.xda-developers.com/m/evildog1.4623971/"));
            softpediaPicBox.Click += new EventHandler((sender, e) => Process.Start("http://www.softpedia.com/get/Programming/Debuggers-Decompilers-Dissasemblers/Apk-Easy-Tool.shtml"));
            #endregion

            //copy context menu
            #region contextmenu
            ContextMenu contextMenu = new ContextMenu();
            MenuItem menuItem = new MenuItem();
            menuItem = new MenuItem("Copy");
            menuItem.Click += new EventHandler(CopyAction);
            contextMenu.MenuItems.Add(menuItem);
            richTextBoxLogs.ContextMenu = contextMenu;
            if (_logform != null)
                _logform.richTextBoxLogsStandalone.ContextMenu = contextMenu;
            #endregion
        }

        #region MainForm
        private void AMF_Load(object sender, EventArgs e)
        {
            mainFormInstance.Init();
            new TaskBarJumpList(Handle);
        }

        private void AMF_LocationChanged(object sender, EventArgs e)
        {
            if (logOutputForm != null)
            {
                logOutputForm.Top = Location.Y;
                logOutputForm.Left = Location.X + Width;
                logOutputForm.Height = Height;
                logOutputForm.BringToFront();
            }
        }

        private void ApkMainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
                tMain.SelectedIndex = 0;
            if (e.KeyCode == Keys.F2)
                tMain.SelectedIndex = 1;
            if (e.KeyCode == Keys.F3)
                tMain.SelectedIndex = 2;
            if (e.KeyCode == Keys.F4)
                tMain.SelectedIndex = 3;
            if (e.KeyCode == Keys.F5)
                tMain.SelectedIndex = 4;
            if (e.KeyCode == Keys.F6)
                tMain.SelectedIndex = 5;
            if (e.KeyCode == Keys.Escape)
                tabMainInstance.Cancel();
        }

        public void DisableForm()
        {
            tMain.Enabled = false;

            if (taskBarCheckBox.Checked)
                TaskBarUtils.TaskBar(1);
        }

        public void EnableForm()
        {
            SystemSounds.Beep.Play();
            tMain.Enabled = true;
            if (taskBarCheckBox.Checked)
            {
                TaskBarUtils.TaskBar(0);
                ExtensionMethods.FlashNotification(this);
            }
        }
        #endregion

        #region TabMain
        private void tMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tMain.SelectedIndex)
            {
                case 2:
                    {
                        richTextBoxLogs.SelectionStart = richTextBoxLogs.Text.Length;
                        richTextBoxLogs.ScrollToCaret();
                    }
                    break;
            }
        }

        private void pathOfApk_SelectedValueChanged(object sender, EventArgs e)
        {
            if (isSelectedFromDD)
                tabMainInstance.pathOfApk_SelectedValueChanged(sender, e);
            isSelectedFromDD = false;
        }

        private void decNameTextBox_Validated(object sender, EventArgs e)
        {
            if (disHisBox.Checked == false)
            {
                if (pathOfApk.Text != "" && !decNameTextBox.Items.Contains(decNameTextBox.Text))
                {
                    decNameTextBox.Items.Insert(0, decNameTextBox.Text);
                }
            }
        }

        private void comNameTextBox_Validated(object sender, EventArgs e)
        {
            if (disHisBox.Checked == false)
            {
                if (pathOfApk.Text != "" && !comNameTextBox.Items.Contains(comNameTextBox.Text))
                {
                    comNameTextBox.Items.Insert(0, comNameTextBox.Text);
                }
            }
        }

        private void pathOfApk_DropDownClosed_1(object sender, EventArgs e)
        {
            isSelectedFromDD = true;
        }

        private void pathOfApk_Validated(object sender, EventArgs e)
        {
            tabMainInstance.pathOfApk_Validated(sender, e);
        }

        private void ApkForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!dontSave) mainFormInstance.SaveConfig();
        }

        //checkbox
        private void useTagChkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (useTagChkBox.Checked)
                tagTxtBox.Enabled = true;
            else
                tagTxtBox.Enabled = false;
        }
        #endregion

        #region TabOptions
        private void clrHisBtn_Click(object sender, EventArgs e)
        {
            tabOptionsInstance.clrHisBtn_Click(sender, e);
        }

        private void langComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (langComboBox.SelectedIndex > 0)
            {
                Lang.LoadLocalization(Variables.RealPath("Language\\" + langComboBox.Text));
                Debug.WriteLine("Lang " + langComboBox.Text);
                LanguageLoaded = true;
            }
            else if (LanguageLoaded)
            {
                Application.Restart();
            }
        }

        private void apkToolComboBox_SelectedValueChanged_1(object sender, EventArgs e)
        {
            tabOptionsInstance.apkToolComboBox_SelectedValueChanged_1(sender, e);
        }

        private void decodeApiLvl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void rebuildApiLvl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void resBtn_Click(object sender, EventArgs e)
        {
            dontSave = true;
            tabOptionsInstance.resBtn_Click(sender, e);
        }
        #endregion

        #region TabLogs
        private void clearLogBtn_Click(object sender, EventArgs e)
        {
            File.Delete(Variables.LogPath);
            richTextBoxLogs.Text = "";
        }

        void CopyAction(object sender, EventArgs e)
        {
            if (richTextBoxLogs.SelectedText == "")
                Clipboard.SetText(_logform.richTextBoxLogsStandalone.SelectedText);
            else
                Clipboard.SetText(richTextBoxLogs.SelectedText);
        }

        private void richTextBoxLogs_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (logOutputForm != null)
                {
                    logOutputForm.richTextBoxLogsStandalone.Text = richTextBoxLogs.Text;
                    //logOutputForm.richTextBoxLogsStandalone.SelectionStart = richTextBoxLogs.Text.Length;
                    //logOutputForm.richTextBoxLogsStandalone.ScrollToCaret();
                }
            }
            catch
            {

            }
        }
        #endregion

        #region Log
        //Color list /*http://www.flounder.com/csharp_color_table.htm*/
        public void LogOutput(string text, Type type = 0, Color? color = null)
        {
            switch (type)
            {
                case Type.None:
                    Log(text, color);
                    Status(text, null);
                    break;
                case Type.Info:
                    Log(text, color);
                    Status(text, Properties.Resources.info);
                    break;
                case Type.Error:
                    Log(text, color);
                    Status(text, Properties.Resources.error);
                    break;
                case Type.Warning:
                    Log(text, color);
                    Status(text, Properties.Resources.warning);
                    break;
                default:
                    break;
            }
        }

        public void LogOutput(string text)
        {
            Log(text);
        }

        public void Log(string text, Color? color = null)
        {
            Invoke(new Action(delegate ()
            {
                richTextBoxLogs.SelectionColor = color ?? Color.White;
                richTextBoxLogs.AppendText(text + Environment.NewLine);
                // richTextBoxLogs.SelectionStart = richTextBoxLogs.Text.Length;
                // richTextBoxLogs.ScrollToCaret();
            }));
        }

        private void logoPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void decLbl_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void labelMain1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabMain_Click(object sender, EventArgs e)
        {

        }

        private void rebuildApiLvl_TextChanged(object sender, EventArgs e)
        {

        }

        private void decodeApiLvl_TextChanged(object sender, EventArgs e)
        {

        }

        private void adad_Click(object sender, EventArgs e)
        {

        }

        private void label96_Click(object sender, EventArgs e)
        {

        }

        private void label95_Click(object sender, EventArgs e)
        {

        }

        private void tagTxtBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void chkAliBtn_Click(object sender, EventArgs e)
        {

        }

        private void comNameTextBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void decNameTextBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pathOfApk_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void openZipApk_Click(object sender, EventArgs e)
        {

        }

        private void acPicBox_Click(object sender, EventArgs e)
        {

        }

        private void apPicBox_Click(object sender, EventArgs e)
        {

        }

        private void sigVer_Click(object sender, EventArgs e)
        {

        }

        private void label97_Click(object sender, EventArgs e)
        {

        }

        private void fullApkInfoBtn_Click(object sender, EventArgs e)
        {

        }

        private void tarLbl_Click(object sender, EventArgs e)
        {

        }

        private void minLbl_Click(object sender, EventArgs e)
        {

        }

        private void vercLbl_Click(object sender, EventArgs e)
        {

        }

        private void verLbl_Click(object sender, EventArgs e)
        {

        }

        private void launchLbl_Click(object sender, EventArgs e)
        {

        }

        private void pakLbl_Click(object sender, EventArgs e)
        {

        }

        private void label54_Click(object sender, EventArgs e)
        {

        }

        private void apkicon_Click(object sender, EventArgs e)
        {

        }

        private void label61_Click(object sender, EventArgs e)
        {

        }

        private void label50_Click(object sender, EventArgs e)
        {

        }

        private void label53_Click(object sender, EventArgs e)
        {

        }

        private void label51_Click(object sender, EventArgs e)
        {

        }

        private void label52_Click(object sender, EventArgs e)
        {

        }

        private void psPicBox_Click(object sender, EventArgs e)
        {

        }

        private void openExt_Click(object sender, EventArgs e)
        {

        }

        private void zipApkBtn_Click(object sender, EventArgs e)
        {

        }

        private void extractApkBtn_Click(object sender, EventArgs e)
        {

        }

        private void comLbl_Click(object sender, EventArgs e)
        {

        }

        private void selectApk_Click(object sender, EventArgs e)
        {

        }

        private void decApkBtn_Click(object sender, EventArgs e)
        {

        }

        private void comApkBtn_Click(object sender, EventArgs e)
        {

        }

        private void installApkBtn_Click(object sender, EventArgs e)
        {

        }

        private void zipAlignBtn_Click(object sender, EventArgs e)
        {

        }

        private void openDecOutput_Click(object sender, EventArgs e)
        {

        }

        private void selectDecDir_Click(object sender, EventArgs e)
        {

        }

        private void openComOutput_Click(object sender, EventArgs e)
        {

        }

        private void signApkBtn_Click(object sender, EventArgs e)
        {

        }

        private void tabSmali_Click(object sender, EventArgs e)
        {

        }

        private void deodexChkBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void smaliNameTxtBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void label90_Click(object sender, EventArgs e)
        {

        }

        private void baksNameTxtBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void label89_Click(object sender, EventArgs e)
        {

        }

        private void panel16_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label87_Click(object sender, EventArgs e)
        {

        }

        private void panel15_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label75_Click(object sender, EventArgs e)
        {

        }

        private void baksChangeBtn_Click(object sender, EventArgs e)
        {

        }

        private void smaliChangeBtn_Click(object sender, EventArgs e)
        {

        }

        private void label70_Click(object sender, EventArgs e)
        {

        }

        private void label71_Click(object sender, EventArgs e)
        {

        }

        private void baksDir_TextChanged(object sender, EventArgs e)
        {

        }

        private void baksSelFileBtn_Click(object sender, EventArgs e)
        {

        }

        private void smaliDir_TextChanged(object sender, EventArgs e)
        {

        }

        private void smaliDisBtn_Click(object sender, EventArgs e)
        {

        }

        private void label69_Click(object sender, EventArgs e)
        {

        }

        private void baksFile_TextChanged(object sender, EventArgs e)
        {

        }

        private void smaliSelFileBtn_Click(object sender, EventArgs e)
        {

        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void smaliFile_TextChanged(object sender, EventArgs e)
        {

        }

        private void smaliAssBtn_Click(object sender, EventArgs e)
        {

        }

        private void openSmaliFolderBtn_Click(object sender, EventArgs e)
        {

        }

        private void openBaksFolderBtn_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox17_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox16_Click(object sender, EventArgs e)
        {

        }

        private void tabLog_Click(object sender, EventArgs e)
        {

        }

        private void tabFw_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void clrFwCacheBtn_Click(object sender, EventArgs e)
        {

        }

        private void installFwBtn_Click(object sender, EventArgs e)
        {

        }

        private void tagFwChkBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void tagFwTxtBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void label86_Click(object sender, EventArgs e)
        {

        }

        private void changeDirFwTxtbox_Click(object sender, EventArgs e)
        {

        }

        private void fwDirTxtBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void label85_Click(object sender, EventArgs e)
        {

        }

        private void changeInsFwTxtbox_Click(object sender, EventArgs e)
        {

        }

        private void installFwDirTxtBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void label84_Click(object sender, EventArgs e)
        {

        }

        private void panel14_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label83_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void pathOfFw_TextChanged(object sender, EventArgs e)
        {

        }

        private void selFramework_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {

        }

        private void openFwDirBtn_Click(object sender, EventArgs e)
        {

        }

        private void tabOpt_Click(object sender, EventArgs e)
        {

        }

        private void oTab_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void tmpFolBtn_Click(object sender, EventArgs e)
        {

        }

        private void useJaxaXmxChkBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void uninsBtn_Click(object sender, EventArgs e)
        {

        }

        private void insBtn_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void taskBarCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void langComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label103_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label37_Click(object sender, EventArgs e)
        {

        }

        private void useExtJava_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void extJavaTxtBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void selJavaBtn_Click(object sender, EventArgs e)
        {

        }

        private void disHisBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void downBtn_Paint(object sender, PaintEventArgs e)
        {

        }

        private void setupDirBtn_Click(object sender, EventArgs e)
        {

        }

        private void restartBtn_Click(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void label78_Click(object sender, EventArgs e)
        {

        }

        private void label77_Click(object sender, EventArgs e)
        {

        }

        private void changeZipDir_Click(object sender, EventArgs e)
        {

        }

        private void pathOfZip_TextChanged(object sender, EventArgs e)
        {

        }

        private void label60_Click(object sender, EventArgs e)
        {

        }

        private void changeExtDir_Click(object sender, EventArgs e)
        {

        }

        private void pathOfExt_TextChanged(object sender, EventArgs e)
        {

        }

        private void folderBtn_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label49_Click(object sender, EventArgs e)
        {

        }

        private void label40_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pathOfCom_TextChanged(object sender, EventArgs e)
        {

        }

        private void winPosCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void pathOfDec_TextChanged(object sender, EventArgs e)
        {

        }

        private void apkToolComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void changeComDir_Click(object sender, EventArgs e)
        {

        }

        private void sevenzComUpDown_ValueChanged(object sender, EventArgs e)
        {

        }

        private void changeDecDir_Click(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label62_Click(object sender, EventArgs e)
        {

        }

        private void javaHeapSizeNum_ValueChanged(object sender, EventArgs e)
        {

        }

        private void chkUpdateChkBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void decForceManifestChkBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void setAaptPathBtn_Click(object sender, EventArgs e)
        {

        }

        private void useAaptPathTxtBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void useAaptPathChkBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void decOnlyMainClassesChkBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void decNoAssetsCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void comNCcheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkBoxUseAapt2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void decFcheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void comFcheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void comCcheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label76_Click(object sender, EventArgs e)
        {

        }

        private void label63_Click(object sender, EventArgs e)
        {

        }

        private void comDcheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void decScheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void decBcheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void decRcheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void decKcheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void decMcheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        private void label30_Click(object sender, EventArgs e)
        {

        }

        private void v3signComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void v4signComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void v2signComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void v4SignLbl_Click(object sender, EventArgs e)
        {

        }

        private void v3SignLbl_Click(object sender, EventArgs e)
        {

        }

        private void v2SignLbl_Click(object sender, EventArgs e)
        {

        }

        private void overApkChecked_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void installApkChkBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void signApkCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label80_Click(object sender, EventArgs e)
        {

        }

        private void jksPass_TextChanged(object sender, EventArgs e)
        {

        }

        private void label73_Click(object sender, EventArgs e)
        {

        }

        private void label72_Click(object sender, EventArgs e)
        {

        }

        private void selJksTxtBox_Click(object sender, EventArgs e)
        {

        }

        private void jksFileTxtBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void label57_Click(object sender, EventArgs e)
        {

        }

        private void useJksCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label56_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void pemFileTxtBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void pk8FileTxtBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void selPemTxtBox_Click(object sender, EventArgs e)
        {

        }

        private void selPk8TxtBox_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {

        }

        private void tabPage6_Click(object sender, EventArgs e)
        {

        }

        private void zipAfterSignChkBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void panel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label81_Click(object sender, EventArgs e)
        {

        }

        private void zipVcheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void zipFcheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void zipZcheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void zipPcheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {

        }

        private void tabAbt_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void label28_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void label26_Click(object sender, EventArgs e)
        {

        }

        private void label25_Click(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void linkLabelXda_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void linkSoft_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void label55_Click(object sender, EventArgs e)
        {

        }

        private void label67_Click(object sender, EventArgs e)
        {

        }

        private void panel12_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel11_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label33_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void softpediaPicBox_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void apkvLbl_Click(object sender, EventArgs e)
        {

        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripStatusLabelImage_Click(object sender, EventArgs e)
        {

        }

        private void toolStripStatusLabelText_Click(object sender, EventArgs e)
        {

        }

        private void saveLogBtn_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = Lang.SAVE_FILE_FILTER = " (*.txt)|*.txt";
                sfd.FileName = "log";
                sfd.FilterIndex = 2;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(sfd.FileName, saveLogBtn.Text);
                }
            }
        }

        private void Status(string message, Image statusImage)
        {
            Invoke(new MethodInvoker(delegate
            {
                toolStripStatusLabelText.Text = message.Replace("\n", "");
                toolStripStatusLabelImage.Image = statusImage;
            }));
        }

        public enum Type
        {
            None,
            Info,
            Error,
            Warning
        }
        #endregion
    }
}