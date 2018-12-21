using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace XMLWizard
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        // Service
        private string _servicename;
        private string _displayname;
        private string _servicedescription;
        // Name
        private ComboBoxItem _projectid;
        private ComboBoxItem _type;
        private string _title;
        private ComboBoxItem _language;
        private string _extension;
        private string _programminglanguage;
        private ComboBoxItem _deletefiles;
        private ComboBoxItem _history;
        private string _localfolder;
        private string _htmlfolder;
        // TFS Only
        private string _name;
        private string _serverurl;
        private string _username;
        //

        public string servicename
        {
            get { return _servicename; }
            set
            {
                _servicename = value;
                OnPropertyChanged("servicename");
            }
        }
        public string displayname
        {
            get { return _displayname; }
            set
            {
                _displayname = value;
                OnPropertyChanged("displayname");
            }
        }
        public string servicedescription
        {
            get { return _servicedescription; }
            set
            {
                _servicedescription = value;
                OnPropertyChanged("servicedescription");
            }
        }

        // Name
        public ComboBoxItem projectid
        {
            get { return _projectid; }
            set
            {
                _projectid = value;
                OnPropertyChanged("projectid");
            }
        }
        public ComboBoxItem type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged("type");
            }
        }
        public string title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged("title");
            }
        }
        public ComboBoxItem language
        {
            get { return _language; }
            set
            {
                _language = value;
                OnPropertyChanged("language");
            }
        }
        public string extension
        {
            get { return _extension; }
            set
            {
                _extension = value;
                OnPropertyChanged("extension");
            }
        }
        public string programminglanguage
        {
            get { return _programminglanguage; }
            set
            {
                _programminglanguage = value;
                OnPropertyChanged("programminglanguage");
            }
        }
        public ComboBoxItem deletefiles
        {
            get { return _deletefiles; }
            set
            {
                _deletefiles = value;
                OnPropertyChanged("deletefiles");
            }
        }
        public ComboBoxItem history
        {
            get { return _history; }
            set
            {
                _history = value;
                OnPropertyChanged("history");
            }
        }
        public string localfolder
        {
            get { return _localfolder; }
            set
            {
                _localfolder = value;
                OnPropertyChanged("localfolder");
            }
        }
        public string htmlfolder
        {
            get { return _htmlfolder; }
            set
            {
                _htmlfolder = value;
                OnPropertyChanged("htmlfolder");
            }
        }
        // TFS Only
        public string name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("name");
            }
        }
        public string serverurl
        {
            get { return _serverurl; }
            set
            {
                _serverurl = value;
                OnPropertyChanged("serverurl");
            }
        }
        public string username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged("username");
            }
        }

        private class ProjectClass
        {
            public ComboBoxItem Type { get; set; }
            public string Title { get; set; }
            public ComboBoxItem Language { get; set; }
            public string Extension { get; set; }
            public string ProgrammingLanguage { get; set; }
            public ComboBoxItem DeleteFiles { get; set; }
            public ComboBoxItem History { get; set; }
            public string LocalFolder { get; set; }
            public string HTMLFolder { get; set; }
            public string Name { get; set; }
            public string ServerURL { get; set; }
            public string Username { get; set; }
        }
        private List<ProjectClass> ProjectList = new List<ProjectClass>();

        XML XMLWizard = new XML();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            for (int i = 0; i <= 4; i++) ProjectList.Add(new ProjectClass());
            LoadXML();
            grpTFS.IsEnabled = (type == null || type.Tag.ToString() == "local") ? false : true;
        }

        #region INotifyPropertyChanged Automation
        protected void OnPropertyChanged(string property)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }
        #endregion

        #region ComboBox - DropDown Events
        private void cboProjectType_DropDownClosed(object sender, System.EventArgs e) => grpTFS.IsEnabled = (type.Tag.ToString() == "local") ? false : true;

        private void cboProjectID_DropDownClosed(object sender, System.EventArgs e)
        {
            int ProjectID = (projectid != null) ? int.Parse(projectid.Tag.ToString()) : -1;

            if (ProjectID == -1) return;

            title = ProjectList[ProjectID].Title;
            type = ProjectList[ProjectID].Type;
            language = ProjectList[ProjectID].Language;
            extension = ProjectList[ProjectID].Extension;
            programminglanguage = ProjectList[ProjectID].ProgrammingLanguage;
            deletefiles = ProjectList[ProjectID].DeleteFiles;
            history = ProjectList[ProjectID].History;
            localfolder = ProjectList[ProjectID].LocalFolder;
            htmlfolder = ProjectList[ProjectID].HTMLFolder;
            name = ProjectList[ProjectID].Name;
            serverurl = ProjectList[ProjectID].ServerURL;
            username = ProjectList[ProjectID].Username;

            grpTFS.IsEnabled = (type == null || type.Tag.ToString() == "local") ? false : true;
        }
        private void cboProjectID_DropDownOpened(object sender, System.EventArgs e) => StoreCurrentProject();
        #endregion

        #region Button - Click Events
        private void Folderpick(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                if ((sender as Button).Name == "btnCodeFolder") localfolder = dialog.FileName;
                else htmlfolder = dialog.FileName;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) => System.Windows.Application.Current.Shutdown();

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StoreCurrentProject();

            string XMLText = "";
            string tab = "	";
            string line = System.Environment.NewLine;
            XMLText += "<?xml version='1.0' encoding='ISO-8859-1'?>" + line;
            XMLText += "<configurations>" + line;

            XMLText += tab + "<service>" + line;
            XMLText += tab + tab + "<servicename>" + servicename + "</servicename>" + line;
            XMLText += tab + tab + "<displayname>" + displayname + "</displayname>" + line;
            XMLText += tab + tab + "<servicedescription>" + servicedescription + "</servicedescription>" + line;
            XMLText += tab + "</service>" + line;

            XMLText += tab + "<projects>" + line;
            int ProjectID = 1;
            foreach(ProjectClass p in ProjectList)
            {
                XMLText += tab + tab + "<project nId='" + ProjectID + "' type='" + ((p.Type == null) ? "" : p.Type.Tag) + "'>" + line;
                XMLText += tab + tab + tab + "<title>" + p.Title + "</title>" + line;
                XMLText += tab + tab + tab + "<language>" + ((p.Language == null) ? "" : p.Language.Tag.ToString().ToLower()) + "</language>" + line;
                XMLText += tab + tab + tab + "<extension>" + p.Extension + "</extension>" + line;
                XMLText += tab + tab + tab + "<programminglanguage>" + p.ProgrammingLanguage + "</programminglanguage>" + line;
                XMLText += tab + tab + tab + "<deletefiles>" + ((p.DeleteFiles == null) ? "" : p.DeleteFiles.Tag) + "</deletefiles>" + line;
                XMLText += tab + tab + tab + "<localfolder>" + p.LocalFolder + "</localfolder>" + line;

                XMLText += tab + tab + tab + "<html>" + line;
                XMLText += tab + tab + tab + tab + "<htmlfolder>" + p.HTMLFolder + "</htmlfolder>" + line;
                XMLText += tab + tab + tab + "</html>" + line;

                if ((p.Type != null && p.Type.Tag.ToString() == "tfvc"))
                {
                    XMLText += tab + tab + tab + "<tfvc>" + line;
                    XMLText += tab + tab + tab + tab + "<name>" + p.Name + "</name>" + line;
                    XMLText += tab + tab + tab + tab + "<history>" + ((p.History == null) ? "" : p.History.Tag) + "</history>" + line;
                    XMLText += tab + tab + tab + tab + "<serverurl>" + p.ServerURL + "</serverurl>" + line;

                    XMLText += tab + tab + tab + tab + "<networkcredential>" + line;
                    XMLText += tab + tab + tab + tab + tab + "<username>" + p.Username + "</username>" + line;
                    XMLText += tab + tab + tab + tab + "</networkcredential>" + line;
                    XMLText += tab + tab + tab + "</tfvc>" + line;
                }

                XMLText += tab + tab + "</project>" + line;
                ProjectID++;
            }
            XMLText += tab + "</projects>" + line;
            XMLText += "</configurations>" + line;

            try
            {
                File.WriteAllText(XMLWizard.ConfigFilePath, XMLText, System.Text.Encoding.GetEncoding("Windows-1252"));
                MessageBox.Show("The Configuration File was successfully saved at: " + Environment.NewLine + XMLWizard.ConfigFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        private void LoadXML()
        {
            // TextBox - Services:
            servicename = XMLWizard.GetXmlElements("/configurations/service/servicename");
            displayname = XMLWizard.GetXmlElements("/configurations/service/displayname");
            servicedescription = XMLWizard.GetXmlElements("/configurations/service/servicedescription");

            for (int i = 5; i >= 1; i--)
            {
                int ProjectID = i - 1;
                System.Xml.XmlNode node = (XMLWizard.ConfigFile.DocumentElement == null)?null: XMLWizard.ConfigFile.DocumentElement.SelectSingleNode("/configurations/projects/project[@nId='" + i + "']");
                if (node != null)
                {
                    foreach (ComboBoxItem c in cboProjectType.Items)
                    {
                        if (c.Tag.ToString() == node.Attributes["type"].InnerText.ToLower()) type = ProjectList[ProjectID].Type = c;
                    }

                    // TextBox - Projects:
                    title = ProjectList[ProjectID].Title = XMLWizard.GetXmlElements("/configurations/projects/project[@nId='" + i + "']/title");
                    extension = ProjectList[ProjectID].Extension = XMLWizard.GetXmlElements("/configurations/projects/project[@nId='" + i + "']/extension");
                    programminglanguage = ProjectList[ProjectID].ProgrammingLanguage = XMLWizard.GetXmlElements("/configurations/projects/project[@nId='" + i + "']/programminglanguage");
                    localfolder = ProjectList[ProjectID].LocalFolder = XMLWizard.GetXmlElements("/configurations/projects/project[@nId='" + i + "']/localfolder");
                    htmlfolder = ProjectList[ProjectID].HTMLFolder = XMLWizard.GetXmlElements("/configurations/projects/project[@nId='" + i + "']/html/htmlfolder");
                    serverurl = ProjectList[ProjectID].ServerURL = XMLWizard.GetXmlElements("/configurations/projects/project[@nId='" + i + "']/tfvc/serverurl");
                    name = ProjectList[ProjectID].Name = XMLWizard.GetXmlElements("/configurations/projects/project[@nId='" + i + "']/tfvc/name");
                    username = ProjectList[ProjectID].Username = XMLWizard.GetXmlElements("/configurations/projects/project[@nId='" + i + "']/tfvc/networkcredential/username");
                    
                    // ComboBox - Projects:
                    foreach (ComboBoxItem c in cboProjectID.Items)
                    {
                        if (c.Tag.ToString() == (i - 1).ToString()) projectid  = c;
                    }

                    string attributestring = XMLWizard.GetXmlElements("/configurations/projects/project[@nId='" + i + "']/language");
                    if (attributestring != null)
                    {
                        foreach (ComboBoxItem c in cboProjectLanguage.Items)
                        {
                            if (c.Tag.ToString().ToLower() == attributestring) language = ProjectList[ProjectID].Language = c;
                        }
                    }

                    attributestring = XMLWizard.GetXmlElements("/configurations/projects/project[@nId='" + i + "']/deletefiles");
                    if (attributestring != null)
                    {
                        foreach (ComboBoxItem c in cboDeleteFiles.Items)
                        {
                            if (c.Tag.ToString() == attributestring) deletefiles = ProjectList[ProjectID].DeleteFiles = c;
                        }
                    }
                    
                    attributestring = XMLWizard.GetXmlElements("/configurations/projects/project[@nId='" + i + "']/tfvc/history");
                    if (attributestring != null)
                    {
                        foreach (ComboBoxItem c in cboShowHistory.Items)
                        {
                            if (c.Tag.ToString() == attributestring) history = ProjectList[ProjectID].History = c;
                        }
                    }
                }
            }
        }    

        private void StoreCurrentProject()
        {
            int ProjectID = (projectid != null) ? int.Parse(projectid.Tag.ToString()) : -1;

            if (ProjectID == -1) return;
            ProjectList[ProjectID].Type = type;
            ProjectList[ProjectID].Title = title;
            ProjectList[ProjectID].Language = language;
            ProjectList[ProjectID].Extension = extension;
            ProjectList[ProjectID].ProgrammingLanguage= programminglanguage;
            ProjectList[ProjectID].DeleteFiles = deletefiles;
            ProjectList[ProjectID].History = history;
            ProjectList[ProjectID].LocalFolder = localfolder;
            ProjectList[ProjectID].HTMLFolder = htmlfolder;
            ProjectList[ProjectID].Name = name;
            ProjectList[ProjectID].ServerURL = serverurl;
            ProjectList[ProjectID].Username = username;
        }
    }
}
