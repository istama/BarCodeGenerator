
Imports MP.Utils.Common

Namespace BarCodeGenerator

  Namespace BCFont
    Public Module NW7
      Private NumberCharList As New List(Of Char) From {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9"}
      Private AlphCharList As New List(Of Char) From {"A", "B", "C", "D"}
      Private SignCharList As New List(Of Char) From {"-", "$", ":", "/", ".", "+"}
      Private CharList As List(Of Char) = UnifyList()

      Public Function IsCharCodeValid(code As Char) As Boolean
        Return CharList.Contains(code)
      End Function

      Public Function IsStrCodeValide(code As String) As Boolean
        If Not AlphCharList.Contains(code.First) OrElse Not AlphCharList.Contains(code.Last) Then
          Return False
        End If

        Dim valid = True

        For Each c As Char In code.ToCharArray()
          If Not IsCharCodeValid(c) Then
            valid = False
            Exit For
          End If
        Next
        Return valid
      End Function

      Private Function UnifyList() As List(Of Char)
        Dim l As New List(Of Char)(NumberCharList)
        l.AddRange(AlphCharList)
        l.AddRange(SignCharList)
        Return l
      End Function

      Public Function CreateFont(size As Integer) As Font
        'PrivateFontCollectionオブジェクトを作成する
        Dim pfc As New System.Drawing.Text.PrivateFontCollection()
        'PrivateFontCollectionにフォントを追加する
        Dim fontFilePath As String = FilePath.GetPath(AppProperties.SETTING, AppProperties.KEY_NW7_FILE_DIR, AppProperties.KEY_NW7_FILE_NAME)
        pfc.AddFontFile(fontFilePath)

        Dim NW7ff As FontFamily = Nothing
        For Each ff As System.Drawing.FontFamily In pfc.Families
          If ff.Name = "NW-7" Then
            NW7ff = ff
            Exit For
          End If
        Next

        If NW7ff IsNot Nothing Then
          'フォントオブジェクトの作成
          Return New Font(NW7ff, size, FontStyle.Regular)
        Else
          Throw New Exception("NW-7が見つかりません。")
        End If
      End Function
    End Module

  End Namespace

  Namespace Graphic

    Public Class BarCodeDrawer
      Private BarCodeFont As Font
      Private CodeFont As Font

      Private PanelFrame As Frame = CreatePanelFrame()
      Private BarCodeFrame1 As Frame = CreateBarCodeFrame1()
      Private BarCodeFrame2 As Frame = CreateBarCodeFrame2()
      Private CodeFrame As Frame = CreateCodeFrame()

      Public Sub New(barCodeFont As Font, codeFont As Font)
        Me.BarCodeFont = barCodeFont
        Me.CodeFont = codeFont
      End Sub

      Public Function Create(code As String) As Panel
        Dim panel As Panel = CreatePanel()
        panel.Controls.Add(CreateBarCodeLabel2(code))
        panel.Controls.Add(CreateBarCodeLabel1(code))
        panel.Controls.Add(CreateCodeLabel(code))
        Return panel
      End Function

      Private Structure Frame
        Dim X As Integer
        Dim Y As Integer
        Dim Width As Integer
        Dim Height As Integer
      End Structure

      Private Function CreatePanel() As Panel
        Dim panel As New Panel
        panel.Top = PanelFrame.Y
        panel.Left = PanelFrame.X
        panel.Width = PanelFrame.Width
        panel.Height = PanelFrame.Height
        Return panel
      End Function

      Private Function CreateBarCodeLabel1(code As String) As Label
        Return CreateLabel(code, BarCodeFont, BarCodeFrame1)
      End Function

      Private Function CreateBarCodeLabel2(code As String) As Label
        Return CreateLabel(code, BarCodeFont, BarCodeFrame2)
      End Function

      Private Function CreateCodeLabel(code As String) As Label
        Return CreateLabel(code, CodeFont, CodeFrame)
      End Function

      Private Function CreateLabel(code As String, font As Font, frame As Frame) As Label
        Dim label As New Label()

        label.Font = font
        label.Top = frame.Y
        Dim size As SizeF = label.CreateGraphics.MeasureString(code, font, PanelFrame.Width + 10)
        label.Left = (PanelFrame.Width - size.Width) / 2
        label.Width = frame.Width
        label.Height = frame.Height
        label.Text = code
        'label.FlatStyle = FlatStyle.Standard
        'label.BackColor = Color.Transparent

        If size.Width > PanelFrame.Width Then
          label.ForeColor = Color.Red
        End If

        Return label
      End Function

      Private Function CreatePanelFrame() As Frame
        Return CreateFrame(4, 4, 177, 75)
      End Function

      Private Function CreateBarCodeFrame1() As Frame
        Return CreateFrame(0, 12, 170, 19)
      End Function

      Private Function CreateBarCodeFrame2() As Frame
        Return CreateFrame(0, 27, 170, 19)
      End Function

      Private Function CreateCodeFrame() As Frame
        Return CreateFrame(0, 46, 170, 19)
      End Function

      Private Function CreateFrame(x As Integer, y As Integer, w As Integer, h As Integer) As Frame
        Dim f As Frame
        With f
          .X = x
          .Y = y
          .Width = w
          .Height = h
        End With
        Return f
      End Function

    End Class




  End Namespace

End Namespace
