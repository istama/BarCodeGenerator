
Imports MP.Details.IO
Imports MP.Details.Sys

Namespace Utils

  Namespace Common
    Public Module MsgBox
      Public Sub ShowWarn(msg As String)
        MessageBox.Show(msg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
      End Sub
      Public Sub ShowWarn(ex As Exception)
        Show(ex, "Warning", MessageBoxIcon.Warning)
      End Sub

      Public Sub ShowError(msg As String)
        MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
      End Sub
      Public Sub ShowError(ex As Exception)
        Show(ex, "Error", MessageBoxIcon.Warning)
      End Sub
      Private Sub Show(ex As Exception, title As String, icon As MessageBoxIcon)
        MessageBox.Show(ex.Message & vbCrLf & vbCrLf & ex.StackTrace, title, MessageBoxButtons.OK, icon)
      End Sub

    End Module

    Public Module FilePath
      'Public Function UserinfoFilePath() As String
      'Return GetPath(AppProperties.SETTING, AppProperties.KEY_USERINFO_FILE_DIR, AppProperties.KEY_USERINFO_FILE_NAME)
      'End Function

      Public Function ScriptForUpdatePath() As String
        Return GetPath(AppProperties.SETTING, AppProperties.KEY_SCRIPT_FOR_UPDATE_DIR, AppProperties.KEY_SCRIPT_FOR_UPDATE_NAME)
      End Function

      Public Function GetPath(entry As AppProperties.Entry, dirKey As String, fileKey As String) As String
        Dim dir As String = entry.GetValue(dirKey)
        Dim file As String = entry.GetValue(fileKey)
        Return dir & "\" & file
      End Function
    End Module

    Public Class AppProperties
      Private Shared SETTING_FILE_NAME = "setting.properties"

      Public Shared KEY_CURRENT_VERSION = "CurrentVersion"
      Public Shared KEY_RELEASE_VERSIONINFO_FILE_DIR = "ReleaseVersioninfoFileDir"
      Public Shared KEY_RELEASE_VERSIONINFO_FILE_NAME = "ReleaseVersioninfoFileName"
      Public Shared KEY_SCRIPT_FOR_UPDATE_DIR = "ScriptForUpdateDir"
      Public Shared KEY_SCRIPT_FOR_UPDATE_NAME = "ScriptForUpdateName"
      Public Shared KEY_NW7_FILE_DIR = "NW7Dir"
      Public Shared KEY_NW7_FILE_NAME = "NW7FileName"

      Public Shared SETTING = New Entry(SETTING_FILE_NAME, DefaultSettingProperties())

      Class Entry
        Private FilePath As String
        Private DefProperties As IDictionary(Of String, String)
        Private Properties As IDictionary(Of String, String) = New Dictionary(Of String, String)

        Private hasRead As Boolean = False

        Public Sub New(filePath As String, def As IDictionary(Of String, String))
          Me.FilePath = filePath
          Me.DefProperties = def
        End Sub

        Public Function GetValue(key As String) As String
          Load()

          If Properties.ContainsKey(key) Then
            Return Properties(key)
          Else
            Reload(DefProperties)
            If Properties.ContainsKey(key) Then
              Return Properties(key)
            Else
              Return ""
            End If
          End If
        End Function

        Private Sub Load()
          If Not hasRead Then
            Try
              Properties = PropertyAccessor.GetProp(FilePath)
            Catch ex As System.IO.FileNotFoundException
              PropertyAccessor.SetProp(FilePath, DefProperties)
              Properties = DefProperties
            Catch ex As Exception
              MessageBox.Show(ex.Message, "Error")
            End Try
            hasRead = True
          End If
        End Sub

        Private Sub Reload(addedProp As IDictionary(Of String, String))
          PropertyAccessor.AppendPropThatNotExistsOnly(FilePath, addedProp)
          hasRead = False
          Load()
        End Sub

      End Class

      Private Shared Function DefaultSettingProperties() As IDictionary(Of String, String)
        Dim setting As IDictionary(Of String, String) = New Dictionary(Of String, String)
        setting(KEY_CURRENT_VERSION) = "1.0.0"
        setting(KEY_RELEASE_VERSIONINFO_FILE_DIR) = ""
        setting(KEY_RELEASE_VERSIONINFO_FILE_NAME) = "version.txt"
        setting(KEY_SCRIPT_FOR_UPDATE_DIR) = App.GetCurrentDirectory()
        setting(KEY_SCRIPT_FOR_UPDATE_NAME) = "update.bat"
        setting(KEY_NW7_FILE_DIR) = App.GetCurrentDirectory()
        setting(KEY_NW7_FILE_NAME) = "NW-7.ttf"
        Return setting
      End Function

    End Class

    Public Module Version
      Public Function CurrentVersion() As String
        Return AppProperties.SETTING.GetValue(AppProperties.KEY_CURRENT_VERSION)
      End Function

      Public Function IsLatestVersion(cmpVer As String) As Boolean
        Dim v As String = CurrentVersion()
        Dim vl As String() = v.Split(".")
        Dim cvl As String() = cmpVer.Split(".")

        Dim isLatest = True
        If vl.Length = cvl.Length Then
          For i As Integer = 0 To (vl.Length - 1)
            If Char.IsDigit(vl(i)) AndAlso Char.IsDigit(cvl(i)) Then
              Dim vi As Integer = Integer.Parse(vl(i))
              Dim cvi As Integer = Integer.Parse(cvl(i))
              If vi > cvi Then
                Exit For
              ElseIf vi < cvi Then
                isLatest = False
                Exit For
              End If
            Else
              Exit For
            End If
          Next
        End If
        Return isLatest
      End Function
    End Module

    Public Module Alph
      Dim aInt As Integer = Asc("A")

      Function ToInt(s As String) As Integer
        If Not Char.IsLetter(s) Then
          Throw New Exception("文字列がアルファベットではありません。" + s)
        End If

        Dim ca = s.ToCharArray()
        Dim sisu = ca.Length - 1

        Dim num = 0
        For Each c As Char In ca
          If (sisu = 0) Then
            num += ToInt(c)
          Else
            num += ToInt(c) * System.Math.Pow(26, sisu)
          End If
          sisu -= 1
        Next

        Return num
      End Function

      Function ToInt(c As Char) As Integer
        Dim cc = UCase(c)
        Return Asc(cc) - aInt + 1
      End Function

      Public Function ToWord(value As Integer) As String
        Const BASE_NUM As Integer = 26

        If value <= BASE_NUM Then
          Return ToChar(value)
        Else
          Dim left As Integer = (value - 1) \ BASE_NUM
          Return ToWord(left) & ToWord(value - (BASE_NUM * left))
        End If
      End Function

      Function ToChar(offset As Integer) As Char
        If offset < 1 OrElse offset > 26 Then
          Throw New Exception("数値が範囲の外です")
        End If

        Dim a As Char = "A"
        Dim aCode As Integer = Asc(a)
        Return Convert.ToChar(offset + aCode - 1)
      End Function
    End Module

  End Namespace
End Namespace