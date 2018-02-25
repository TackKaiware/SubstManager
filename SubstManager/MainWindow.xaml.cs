using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace SubstManager
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Drive> _drives = new List<Drive>();

        public MainWindow()
        {
            InitializeComponent();

            // ドライブ情報を取得
            var driveNames = Enumerable.Range( 'A', ( 'Z' - 'A' + 1 ) )
                          .Select( x => Convert.ToChar( x ).ToString() + ':' )
                          .ToList();

            foreach ( var name in driveNames )
            {
                _drives.Add( new Drive( name ) );
            }

            // DataGridに設定する
            dataGrid.ItemsSource = _drives;
        }

        /// <summary>
        /// DataGridのカラム名を自動生成する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGrid_AutoGeneratingColumn( object sender, DataGridAutoGeneratingColumnEventArgs e )
        {
            switch ( e.PropertyName )
            {
                case nameof( Drive.Name ):
                    e.Column.Header = "ドライブ名";
                    e.Column.DisplayIndex = 0;
                    break;

                case nameof( Drive.Status ):
                    e.Column.Header = "ドライブの状態";
                    e.Column.DisplayIndex = 1;
                    break;

                case nameof( Drive.Description ):
                    e.Column.Header = "割り当てているフォルダ";
                    e.Column.DisplayIndex = 2;
                    break;

                default:
                    throw new InvalidProgramException();
            }
        }

        /// <summary>
        /// DataGridのセルが選択された時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGrid_SelectedCellsChanged( object sender, SelectedCellsChangedEventArgs e )
        {
            if ( dataGrid.SelectedItem is Drive drive )
            {
                if ( drive.Status != DriveStatus.Disable )
                {
                    var dialog = new FolderBrowserDialog
                    {
                        RootFolder = Environment.SpecialFolder.Desktop,
                        Description = $"ドライブ({drive.Name})に割り当てるフォルダを選択",
                        ShowNewFolderButton = false,
                    };

                    dialog.ShowDialog();
                    var folderPath = dialog.SelectedPath;
                    drive.Assign( folderPath );
                }
            }
        }
    }
}