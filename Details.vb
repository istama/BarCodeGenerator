
Namespace Details
  Namespace IO
    Public Module PropertyAccessor
      Private SEPARATOR As Char = "="

      Public Function GetProp(fileName As String) As IDictionary(Of String, String)
        Dim pp As IDictionary(Of String, ArrayList) = GetPropDuplicatedKeys(fileName)

        Dim p As New Dictionary(Of String, String)
        For Each k In pp.Keys
          p.Add(k, pp(k)(0))
        Next

        Return p
      End Function

      Public Function GetPropDuplicatedKeys(fileName As String) As IDictionary(Of String, ArrayList)
        Dim p As New Dictionary(Of String, ArrayList)
        Dim texts As List(Of String) = FileAccessor.Read(fileName)

        For Each t As String In texts
          Dim idx As Integer = t.IndexOf(SEPARATOR)
          If idx > 0 Then
            Dim key As String = t.Substring(0, idx)
            Dim value As String = t.Substring(idx + 1)
            If Not p.ContainsKey(key) Then
              p.Add(key, New ArrayList)
            End If
            p(key).Add(value)
          End If
        Next

        Return p
      End Function

      Public Sub SetProp(fileName As String, p As IDictionary(Of String, String))
        FileAccessor.Write(fileName, DictToPropList(p, Nothing))
      End Sub

      Public Sub AppendProp(fileName As String, key As String, value As String)
        Dim l As New List(Of String)(New String() {ToPropString(key, value)})
        FileAccessor.Append(fileName, l)
      End Sub

      Public Sub AppendProp(fileName As String, p As IDictionary(Of String, String))
        FileAccessor.Append(fileName, DictToPropList(p, Nothing))
      End Sub

      Public Sub AppendPropThatNotExistsOnly(fileName As String, p As IDictionary(Of String, String))
        Dim keys As ICollection(Of String) = GetPropDuplicatedKeys(fileName).Keys
        'Dim k As IDictionary(Of String, ArrayList) = GetPropDuplicatedKeys(fileName).Keys.ToList
        FileAccessor.Append(fileName, DictToPropList(p, keys))
      End Sub

      Private Function DictToPropList(dict As IDictionary(Of String, String), omittedKeys As ICollection(Of String))
        Dim l As New List(Of String)

        For Each k As String In dict.Keys
          If omittedKeys Is Nothing OrElse Not omittedKeys.Contains(k) Then
            l.Add(ToPropString(k, dict(k)))
          End If
        Next

        Return l
      End Function

      Private Function ToPropString(key As String, value As String) As String
        Return key & SEPARATOR & value
      End Function
    End Module

    Public Module FileAccessor
      Public Function Read(fileName As String) As List(Of String)
        Dim op As New Op()
        Dim f As Operate = AddressOf op.Input
        Return Access(fileName, OpenMode.Input, f)
      End Function

      Public Sub Write(fileName As String, text As String)
        Dim l As New List(Of String)(New String() {text})
        Write(fileName, l, OpenMode.Output)
      End Sub

      Public Sub Write(fileName As String, text As List(Of String))
        Write(fileName, text, OpenMode.Output)
      End Sub

      Public Sub Append(fileName As String, text As String)
        Dim l As New List(Of String)(New String() {text})
        Write(fileName, l, OpenMode.Append)
      End Sub

      Public Sub Append(fileName As String, text As List(Of String))
        Write(fileName, text, OpenMode.Append)
      End Sub

      Private Sub Write(fileName As String, text As List(Of String), mode As Integer)
        Dim op As New Op(text)
        Dim f As Operate = AddressOf op.Output
        Access(fileName, mode, f)
      End Sub

      Private Function Access(fileName As String, mode As Integer, op As Operate) As List(Of String)
        Dim text As List(Of String)
        Dim fh As Integer = FreeFile()
        Try
          FileOpen(fh, fileName, mode)
          text = op(fh)
        Finally
          FileClose(fh)
        End Try

        Return text
      End Function

      Private Class Op
        Private WrittingText As New List(Of String)

        Sub New()
        End Sub

        Sub New(text As List(Of String))
          Me.WrittingText = text
        End Sub

        Function Output(fh As Integer) As List(Of String)
          For Each t As String In WrittingText
            PrintLine(fh, t)
          Next

          Return Nothing
        End Function

        Function Input(fh As Integer) As List(Of String)
          Dim list As New List(Of String)

          Do While EOF(fh) = False
            Dim l As String = LineInput(fh)
            list.Add(l)
          Loop

          Return list
        End Function
      End Class

      Delegate Function Operate(fh As Integer) As List(Of String)
    End Module

  End Namespace

  Namespace Sys
    Public Module App
      Public Function FrameworkVersionNumber() As Double
        Dim verStr As String = System.Reflection.Assembly.GetExecutingAssembly().ImageRuntimeVersion
        Return Double.Parse(verStr.Substring(1, 3))
      End Function

      Public Function GetCurrentDirectory() As String
        Return System.IO.Directory.GetCurrentDirectory()
      End Function
    End Module
  End Namespace

End Namespace