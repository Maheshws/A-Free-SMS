Imports mshtml
Imports System.Threading

Public Class Home
    Dim Country As String
    Dim CountryURL As String
    Dim SAPI


    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        MsgBox("Make sure to enter your mobile destination number in this format, for instance for SMS to Costa Rica mobile: +506xxxxxxxxx.(You only need to fill the (x) where 'xxxxxxxxx' is the SMS To mobile number WITHOUT the first leading (0) zero and the country code).")
    End Sub

    Private Sub WebBrowser1_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles WebBrowser1.DocumentCompleted
        Label1.Text = "SMS will not be delivered if DND is active"
        Label1.ForeColor = Color.Red
        Dim CD As IHTMLDocument2 = WebBrowser1.Document.DomDocument
        Dim Images As IHTMLControlRange = CType(CD.body, HTMLBody).createControlRange
        For Each imgs As IHTMLImgElement In CD.images
            If imgs.GetAttribute("src").ToString.Contains("/image.php") Then
                Images.add(imgs)
                Images.execCommand("Copy", False, Nothing)
                PictureBox1.Image = Clipboard.GetDataObject.GetData(DataFormats.Bitmap)
                Exit For
            End If
        Next

        If WebBrowser1.Document.GetElementById("smsfrom") IsNot Nothing Then
            TextBox1.Enabled = True
        Else
            TextBox1.Enabled = False
        End If
        Try
            Dim countrycode As String = WebBrowser1.Document.GetElementById("countrycode").GetAttribute("value")
            Label5.Text = "+" & countrycode
            TextBox1.Text = countrycode

            Button1.Enabled = True
        Catch ex As Exception

        End Try

    End Sub


    Private Sub Form1_Load(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Country = ComboBox1.SelectedItem
        Try
            If Country.Contains(" ") Then
                CountryURL = Replace(Country, " ", "-")
                WebBrowser1.Navigate("http://afreesms.com/intl/" & CountryURL)
            Else
                WebBrowser1.Navigate("http://afreesms.com/intl/" & Country)
            End If
            TextBox1.Enabled = True
            TextBox2.Enabled = True
            TextBox3.Enabled = True
            TextBox4.Enabled = True
        Catch
            MessageBox.Show("Invalid Country", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox2.Text.Length > 20 Then
            If TextBox1.Enabled = True Then
                FillWithInformation2()
            Else
                FillWithInformation1()
            End If
        Else
            Dim thread As New Thread(
            Sub()
                SAPI.Speak("Message needs to be more than 20 characters")
            End Sub
            )
            thread.Start()

            MsgBox("Needs to be more than 20 character")
        End If

    End Sub


    Sub FillWithInformation1()
        Try
            WebBrowser1.Document.GetElementById("smsto").SetAttribute("value", TextBox3.Text)
            WebBrowser1.Document.GetElementById("message").SetAttribute("value", TextBox2.Text)
            WebBrowser1.Document.GetElementById("imgcode").SetAttribute("value", TextBox4.Text)
            WebBrowser1.Document.GetElementById("submit").InvokeMember("click")
            Dim thread As New Thread(
                Sub()
                    SAPI.Speak("Message Successfully Sent, but not guaranteed if delivered")
                End Sub
            )
            Thread.Start()
            MessageBox.Show("Message Successfully Sent, but not guaranteed if delivered", "Success", MessageBoxButtons.OK, MessageBoxIcon.None)
            ClearEverything()
        Catch ex As Exception
            MsgBox("Error")
            ClearEverything()
        End Try

    End Sub
    Sub FillWithInformation2()
        Try
            WebBrowser1.Document.GetElementById("smsto").SetAttribute("value", TextBox3.Text)
            WebBrowser1.Document.GetElementById("message").SetAttribute("value", TextBox2.Text)
            WebBrowser1.Document.GetElementById("imgcode").SetAttribute("value", TextBox4.Text)
            WebBrowser1.Document.GetElementById("smsfrom").SetAttribute("value", TextBox1.Text)
            WebBrowser1.Document.GetElementById("submit").InvokeMember("click")
            Dim thread As New Thread(
                Sub()
                    SAPI.Speak("Message Successfully Sent, but not guaranteed if delivered")
                End Sub
            )
            thread.Start()
            MessageBox.Show("Message Successfully Sent, but not guaranteed if delivered", "Success", MessageBoxButtons.OK, MessageBoxIcon.None)
            ClearEverything()
        Catch ex As Exception
            MsgBox("Error")
            ClearEverything()
        End Try

    End Sub

    Sub ClearEverything()
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        Label5.Text = ""
        Try
            PictureBox1.Image.Dispose()
            PictureBox1.Image = Nothing
        Catch ex As Exception
        End Try

    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        System.Diagnostics.Process.Start("http://www.afreesms.com")
    End Sub

    Private Sub LinkLabel3_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
        System.Diagnostics.Process.Start("http://maxxroid.com/")
    End Sub

    Private Sub Home_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim thread As New Thread(
               Sub()
                   SAPI = CreateObject("SAPI.spvoice")
                   SAPI.Speak("Welcome")
               End Sub
           )
        thread.Start()
       
    End Sub
End Class
