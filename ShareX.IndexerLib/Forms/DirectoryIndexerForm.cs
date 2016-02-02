﻿#region License Information (GPL v3)

/*
    ShareX - A program that allows you to take screenshots and share any file type
    Copyright (c) 2007-2016 ShareX Team

    This program is free software; you can redistribute it and/or
    modify it under the terms of the GNU General Public License
    as published by the Free Software Foundation; either version 2
    of the License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

    Optionally you can also view the license at <http://www.gnu.org/licenses/>.
*/

#endregion License Information (GPL v3)

using ShareX.HelpersLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShareX.IndexerLib
{
    public partial class DirectoryIndexerForm : BaseForm
    {
        public delegate void UploadRequestedEventHandler(string source);
        public event UploadRequestedEventHandler UploadRequested;

        public IndexerSettings Settings { get; set; }
        public string Source { get; private set; }

        public DirectoryIndexerForm(IndexerSettings settings)
        {
            InitializeComponent();
            Settings = settings;
            pgSettings.SelectedObject = Settings;
            BrowseFolder();
        }

        private void btnBrowseFolder_Click(object sender, EventArgs e)
        {
            BrowseFolder();
        }

        private void BrowseFolder()
        {
            // Translate
            if (Helpers.BrowseFolder("ShareX - Choose folder path", txtFolderPath))
            {
                IndexFolder();
            }
        }

        private void txtFolderPath_TextChanged(object sender, EventArgs e)
        {
            btnIndexFolder.Enabled = txtFolderPath.TextLength > 0;
        }

        private void btnIndexFolder_Click(object sender, EventArgs e)
        {
            IndexFolder();
        }

        private void IndexFolder()
        {
            string folderPath = txtFolderPath.Text;

            if (!string.IsNullOrEmpty(folderPath) && Directory.Exists(folderPath))
            {
                btnUpload.Enabled = false;
                Source = Indexer.Index(folderPath, Settings);

                if (!string.IsNullOrEmpty(Source))
                {
                    tcMain.SelectedTab = tpPreview;
                    wbMain.DocumentText = Source;
                    btnUpload.Enabled = true;
                }
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Source))
            {
                OnUploadRequested(Source);
                Close();
            }
        }

        protected void OnUploadRequested(string source)
        {
            if (UploadRequested != null)
            {
                UploadRequested(source);
            }
        }
    }
}