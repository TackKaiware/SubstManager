using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace SubstManager
{
    /// <summary>
    /// ローカルフォルダを割り当てるドライブ
    /// </summary>
    public class Drive : INotifyPropertyChanged
    {
        private static readonly string MSG_NO_ASSIGNMENT = "---------- 割り当て無し ----------";
        private string _description;
        private DriveStatus _staus;

        /// <summary>
        /// ローカルフォルダを割り当てるドライブオブジェクトを新規作成します。
        /// </summary>
        /// <param name="driveName"></param>
        public Drive( string driveName )
        {
            if ( string.IsNullOrEmpty( driveName ) || !new Regex( @"[A-Z:\]" ).IsMatch( driveName ) )
            {
                throw new ArgumentException();  // ドライブ名が不正
            }

            Name = driveName;
            Description = WinApiNativeMethods.GetDriveDescription( driveName ) ?? MSG_NO_ASSIGNMENT;
            Status = GetDriveStatus( driveName );
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// ドライブの説明
        /// </summary>
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged( nameof( Description ) );
            }
        }

        /// <summary>
        /// ドライブ名
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// ドライブの状態
        /// </summary>
        public DriveStatus Status
        {
            get => _staus;
            set
            {
                _staus = value;
                OnPropertyChanged( nameof( Status ) );
            }
        }

        /// <summary>
        /// 指定したフォルダをドライブに割り当てる
        /// </summary>
        /// <param name="folderPath"></param>
        public void Assign( string folderPath )
        {
            if ( IsAssignable( this, folderPath ) )
            {
                WinApiNativeMethods.AssignDrive( Name, folderPath );
                Description = folderPath;
            }
            else
            {
                WinApiNativeMethods.UnassignDrive( Name );
                Description = MSG_NO_ASSIGNMENT;
            }

            Status = GetDriveStatus( Name );
        }

        

        /// <summary>
        /// ドライブの状態を取得する。
        /// </summary>
        /// <param name="driveName"></param>
        /// <returns></returns>
        private DriveStatus GetDriveStatus( string driveName )
        {
            var driveInfo = new DriveInfo( driveName );

            // 未使用 -> 使用可能
            if ( !driveInfo.IsReady ) return DriveStatus.Enable;

            var attributes = driveInfo.RootDirectory.Attributes;
            if ( ( ( attributes & FileAttributes.System ) == FileAttributes.System ) ||
                 ( driveInfo.DriveType == DriveType.CDRom ) )
            {
                // ハードディスクやCDドライブ -> 使用不可
                return DriveStatus.Disable;
            }
            else if ( ( attributes & FileAttributes.Directory ) == FileAttributes.Directory )
            {
                // 使用中
                return DriveStatus.Busy;
            }
            else
            {
                // 不明
                return DriveStatus.Unknown;
            }
        }

        /// <summary>
        /// 割り当て可能か
        /// </summary>
        /// <param name="driveStatus"></param>
        /// <returns></returns>
        private bool IsAssignable( Drive drive, string folderPath )
        {
            var isDriveEnable = drive.Status == DriveStatus.Enable;
            var isDriveBusy = drive.Status == DriveStatus.Busy;
            var isDirExists = Directory.Exists( folderPath );

            return ( ( isDriveEnable || isDriveBusy ) && isDirExists );
        }

        /// <summary>
        /// プロパティを変更した場合は、
        /// このメソッドを呼び出してクライアントに変更の通知をする
        /// </summary>
        /// <param name="propertyName"></param>
        private void OnPropertyChanged( string propertyName )
            => PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
    }
}