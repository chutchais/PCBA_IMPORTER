﻿Imports System.Xml

Public Class Form1
    Private objFits As clsFits
    Private objAutoTest As clsAutoTest
    Dim cn As New ADODB.Connection()
    Dim cnAutoTest As New ADODB.Connection()
    Dim objInI As clsINI
    Dim vWorkingDir As String
    Dim vServiceURL As String

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnDatabase.Click
        'Dim objFits As New clsFits
        Try
            If btnDatabase.Text = "&Disconnect Database." Then
                cn.Close()
                cnAutoTest.Close()
                initialControl()
            Else
                tssDatabase.Text = "Connecting Database..." : Application.DoEvents()

                objFits = New clsFits
                With objFits
                    .user = objInI.GetString("database", "user", "")
                    .password = objInI.GetString("database", "password", "")
                    .server = objInI.GetString("database", "server", "")
                    .database = objInI.GetString("database", "database", "")
                    cn = .connect()
                    tssDatabase.Text = "(" & .server & "/" & .database & ")Database connected." : Application.DoEvents()
                End With

                'Open AutoTest database
                objAutoTest = New clsAutoTest
                With objAutoTest
                    .user = objInI.GetString("test database", "user", "")
                    .password = objInI.GetString("test database", "password", "")
                    .server = objInI.GetString("test database", "server", "")
                    .database = objInI.GetString("test database", "database", "")
                    cnAutoTest = .connect()
                End With

                btnDatabase.Text = "&Disconnect Database."
                btnImport.Enabled = True
            End If

        Catch ex As Exception
            MsgBox("Unable to connect database!!!" & vbCrLf & _
                "Because " & ex.Message, MsgBoxStyle.Critical, "Unable to connect database")
            tssDatabase.Text = "Database error!" : Application.DoEvents()
            initialControl()
        End Try



        'Query Data
        'Dim rs As New ADODB.Recordset
        'rs = objFits.getEvents("2016-07-01", "2016-07-02")

        'Dim vAllRecord As Integer = rs.RecordCount
        'Dim vFirstSN As String = rs.Fields("serial_no").Value

        'rs = objFits.getEvents(vFirstSN)
        'Dim vAllFirstRecord As Integer = rs.RecordCount

        'Dim vAttCode As Integer = rs.Fields("sn_attr_code").Value
        'Dim vTransSeq As Integer = rs.Fields("trans_seq").Value

        'rs = objFits.getParameters(vFirstSN, vAttCode, vTransSeq)

        'Dim vAllParamRecord As Integer = rs.RecordCount

        'MsgBox("Total record(s) is " & vAllRecord & vbCrLf & _
        '       "First SN is " & vFirstSN & vbCrLf & _
        '       "record(s) is " & vAllFirstRecord & vbCrLf & _
        '       "all Parameter record : " & vAllParamRecord)



    End Sub


    '    '###Look at Acacia Event table first.
    '    Sub ExportData()
    '        Dim vBullEyesObj As New clsBullEyes
    '        With vBullEyesObj
    '            'Query Data
    '            Dim rs As New ADODB.Recordset
    '            Dim vNewFromDate As Date = CDate(lblFrom.Text).AddSeconds(1)
    '            Dim vDateFrom As String = vNewFromDate.ToString
    '            Dim vDateTo As String = lblTo.Text

    '            'rs = objFits.getPCBAlist(vDateFrom, vDateTo)
    '            rs = objFits.getEvents(vDateFrom, vDateTo)

    '            If rs.RecordCount = 0 Then
    '                lblLastDate.Text = lblNextRun.Text : Application.DoEvents()
    '            End If

    '            Do While Not rs.EOF
    '                tssStatus.Text = "Importing......." & rs.AbsolutePosition & "/" & rs.RecordCount : Application.DoEvents()
    '                .operation = rs.Fields("operation").Value
    '                .datetimeout = IIf(IsDBNull(rs.Fields("date_time_checkout").Value), "", rs.Fields("date_time_checkout").Value)

    '                If Not (.operation = "30" Or .operation = "120" Or .operation = "1100") Then
    '                    GoTo nextSN
    '                End If

    '                .serialnumber = rs.Fields("serial_no").Value
    '                .sn_attr_code = rs.Fields("sn_attr_code").Value
    '                .trans_seq = rs.Fields("trans_seq").Value



    '                Dim vPCBSN As String = ""
    '                Dim vPCBPn As String = ""
    '                Dim vPCBRev As String = ""
    '                Dim vAtrrRst As ADODB.Recordset
    '                vAtrrRst = objFits.getParameters(.serialnumber, .sn_attr_code, .trans_seq)
    '                Do While Not vAtrrRst.EOF
    '                    If vAtrrRst.Fields("attribute_code").Value = "302" Then
    '                        vPCBSN = vAtrrRst.Fields("attribute_value").Value
    '                    End If
    '                    If vAtrrRst.Fields("attribute_code").Value = "110001" Then
    '                        vPCBPn = vAtrrRst.Fields("attribute_value").Value
    '                    End If
    '                    If vAtrrRst.Fields("attribute_code").Value = "110002" Then
    '                        vPCBrev = vAtrrRst.Fields("attribute_value").Value
    '                    End If
    '                    vAtrrRst.MoveNext()
    '                Loop
    '                If vPCBSN = "" Then
    '                    GoTo nextSN
    '                End If

    '                '-----Get PCB Info -----
    '                'History (only EBT)
    '                Dim vPCBHistory As ADODB.Recordset
    '                vPCBHistory = objFits.getPCBAlist(vPCBSN)
    '                'Comp.onent tracking (Only value is not 0)
    '                If vPCBHistory.RecordCount = 0 Then
    '                    GoTo nextSN
    '                End If
    '                vPCBHistory.MoveLast()

    '                Dim vEBTstation As String = vPCBHistory.Fields("operation").Value
    '                .operation = vEBTstation

    '                Dim vDateOutPCB As String = vPCBHistory.Fields("date_time").Value
    '                Dim vTesterPCB As String = "ATE_AC100M_062"
    '                '-----------------------
    '                'Modify all Parameter
    '                Dim vTempTimeOut As String = .datetimeout

    '                .serialnumber = vPCBSN
    '                .workorder = vPCBHistory.Fields("workorder").Value
    '                .model = "PCBA" 'set family

    '                .partnumber = vPCBPn 'rs.Fields("part_no").Value
    '                .operation = vEBTstation 'rs.Fields("operation").Value
    '                .operationname = IIf(IsDBNull(vPCBHistory.Fields("description").Value), "", vPCBHistory.Fields("description").Value)
    '                .buildtype = "PROD" 'rs.Fields("buildtype").Value
    '                .runtype = "100" 'rs.Fields("runtype").Value
    '                .employee = vPCBHistory.Fields("emp_no").Value
    '                .sn_attr_code = rs.Fields("sn_attr_code").Value
    '                .trans_seq = rs.Fields("trans_seq").Value 'vPCBHistory.Fields("date_time").Value
    '                .datetimein = vPCBHistory.Fields("date_time").Value
    '                .datetimeout = vPCBHistory.Fields("date_time").Value
    '                .shift = "DAY" 'rs.Fields("shift").Value
    '                .tester = "" 'rs.Fields("equip_id").Value
    '                .outputPath = vWorkingDir
    '                .result = "PASS" 'IIf(IsDBNull(rs.Fields("disp_code").Value), "PASS", rs.Fields("disp_code").Value)
    '                .disposecode = "PASS" 'IIf(IsDBNull(rs.Fields("disp_code").Value), "", rs.Fields("disp_code").Value)
    '                .next_operation = "382" ' objFits.getNextStation(.serialnumber, .operation, .trans_seq, .model)
    '                'End Modify




    '                'Get Testing Data
    '                '1)get Process from BullsEye -- by Station.
    '                Dim vProcess As String = requestData(vServiceURL & "production/station/" & .operation & "/" & .model & "/")
    '                '2)get Measurement data.
    '                Dim vTestDataRst As New ADODB.Recordset


    '                If vProcess <> "" And vProcess <> "None" Then
    '                    vTestDataRst = objAutoTest.getTestData(.serialnumber, vProcess, .tester, .datetimeout)
    '                Else
    '                    vTestDataRst = Nothing
    '                End If

    '                'Get Component--
    '                Dim vRstComponent As ADODB.Recordset
    '                vRstComponent = objFits.getComponentData(.serialnumber)
    '                If vRstComponent.RecordCount > 0 Then
    '                    tssStatus.Text = "Uploading component data......of " & .serialnumber : Application.DoEvents()
    '                End If
    '                '---------------



    '                '.makeXML(Nothing, vTestDataRst)
    '                .makeXML(objFits.getParameters(.serialnumber, .sn_attr_code, .trans_seq), vTestDataRst, vRstComponent)



    '                uploadData(.outputfile)
    '                .datetimeout = vTempTimeOut

    'nextSN:
    '                lblLastDate.Text = .datetimeout : Application.DoEvents()
    '                '---save last date to INI file---
    '                objInI.WriteString("Last execution", "date", .datetimeout)
    '                '--------------------------------
    '                rs.MoveNext()
    '            Loop
    '        End With
    '        '---Update From/To date
    '        lblFrom.Text = lblLastDate.Text
    '        lblTo.Text = getDateTo(lblLastDate.Text)
    '        lblNextRun.Text = Now.AddMinutes(Val(objInI.GetString("import", "interval", "")))

    '    End Sub


    '##Look at vw_SMTUnitHistoryTracking First.
    Sub ExportData()
        Dim vBullEyesObj As New clsBullEyes
        With vBullEyesObj
            'Query Data
            Dim rs As New ADODB.Recordset
            Dim vNewFromDate As Date = CDate(lblFrom.Text).AddSeconds(1)
            Dim vDateFrom As String = vNewFromDate.ToString
            Dim vDateTo As String = lblTo.Text

            rs = objFits.getPCBAlist(vDateFrom, vDateTo)
            'rs = objFits.getEvents(vDateFrom, vDateTo)

            If rs.RecordCount = 0 Then
                lblLastDate.Text = Now() : Application.DoEvents() ' lblNextRun.Text 
            End If

            Do While Not rs.EOF
                tssStatus.Text = "Importing......." & rs.AbsolutePosition & "/" & rs.RecordCount : Application.DoEvents()
                .operation = rs.Fields("operation").Value

                .datetimeout = IIf(IsDBNull(rs.Fields("date_time").Value), "", rs.Fields("date_time").Value)

                If Not (.operation = "3261") Then
                    GoTo nextSN
                End If

                '.serialnumber = rs.Fields("serial_no").Value
                '.sn_attr_code = rs.Fields("sn_attr_code").Value
                '.trans_seq = rs.Fields("trans_seq").Value



                '-----Get PCB Info -----
                ''History (only EBT)
                'Dim vPCBHistory As ADODB.Recordset
                Dim vPCBSN As String = rs.Fields("serial_no").Value

                'vPCBHistory = objFits.getPCBAlist(vPCBSN)
                ''Comp.onent tracking (Only value is not 0)
                'If vPCBHistory.RecordCount = 0 Then
                '    GoTo nextSN
                'End If
                'vPCBHistory.MoveLast()

                'Dim vEBTstation As String = vPCBHistory.Fields("operation").Value
                '.operation = vEBTstation

                'Dim vDateOutPCB As String = vPCBHistory.Fields("date_time").Value
                Dim vTesterPCB As String = "ATE_AC100M_062"
                '-----------------------
                'Modify all Parameter
                Dim vTempTimeOut As String = .datetimeout

                .serialnumber = vPCBSN
                .workorder = rs.Fields("workorder").Value
                .model = "PCBA" 'set family

                .partnumber = rs.Fields("part_no").Value
                .operation = rs.Fields("operation").Value
                .operationname = IIf(IsDBNull(rs.Fields("description").Value), "", rs.Fields("description").Value)
                .buildtype = "PROD" 'rs.Fields("buildtype").Value
                .runtype = "100" 'rs.Fields("runtype").Value
                .employee = rs.Fields("emp_no").Value
                .sn_attr_code = "1001" 'rs.Fields("sn_attr_code").Value
                .trans_seq = rs.Fields("trans_seq").Value 'vPCBHistory.Fields("date_time").Value
                .datetimein = rs.Fields("date_time").Value
                .datetimeout = rs.Fields("date_time").Value
                .shift = "DAY" 'rs.Fields("shift").Value
                .tester = vTesterPCB 'rs.Fields("equip_id").Value
                .outputPath = vWorkingDir
                .result = IIf(IsDBNull(rs.Fields("result").Value), "PASS", rs.Fields("result").Value)
                .disposecode = IIf(IsDBNull(rs.Fields("result").Value), "", rs.Fields("result").Value)
                .next_operation = "382" ' objFits.getNextStation(.serialnumber, .operation, .trans_seq, .model)
                'End Modify




                'Get Testing Data
                '1)get Process from BullsEye -- by Station.
                Dim vProcess As String = requestData(vServiceURL & "production/station/" & .operation & "/" & .model & "/")
                '2)get Measurement data.
                Dim vTestDataRst As New ADODB.Recordset


                If vProcess <> "" And vProcess <> "None" Then
                    vTestDataRst = objAutoTest.getTestData(.serialnumber, vProcess, .tester, .datetimeout)
                    '.tester = vTestDataRst.Fields("STATION_ID").Value
                Else
                    vTestDataRst = Nothing
                End If

                'Get Component--
                Dim vRstComponent As ADODB.Recordset
                vRstComponent = objFits.getComponentData(.serialnumber)
                If vRstComponent.RecordCount > 0 Then
                    tssStatus.Text = "Uploading " & .serialnumber & " " & rs.AbsolutePosition & "/" & rs.RecordCount : Application.DoEvents()
                End If
                '---------------



                '.makeXML(Nothing, vTestDataRst)
                .makeXML(objFits.getParameters(.serialnumber, .sn_attr_code, .trans_seq), vTestDataRst, vRstComponent)



                uploadData(.outputfile)
                .datetimeout = vTempTimeOut

nextSN:
                lblLastDate.Text = .datetimeout : Application.DoEvents()
                '---save last date to INI file---
                objInI.WriteString("Last execution", "date", .datetimeout)
                '--------------------------------
                rs.MoveNext()
            Loop
        End With
        '---Update From/To date
        lblFrom.Text = lblLastDate.Text
        '---save last date to INI file---
        objInI.WriteString("Last execution", "date", lblLastDate.Text)
        '--------------------------------
        lblTo.Text = getDateTo(lblLastDate.Text)
        lblNextRun.Text = Now.AddMinutes(Val(objInI.GetString("import", "interval", "")))

    End Sub


    Function uploadData(vFile As String) As Boolean
        'HTTP variable
        Dim myHTTP As New MSXML.XMLHTTPRequest
        Dim doc As XmlDocument = New XmlDocument()
        Dim strReturn As String
        uploadData = False
        Try
            doc.Load(vFile)
            With myHTTP
                .open("Post", vServiceURL & "production/fits/upload/", False)
                .setRequestHeader("Content-Type", "text/xml")
                .send(doc.InnerXml) '+64
                strReturn = .responseText
                If strReturn <> """Successful""" Then
                    MsgBox(strReturn)
                End If
            End With
            Return True
        Catch ex As Exception

            MsgBox("Unable to upload XML!!!" & vbCrLf & _
                "Because " & ex.Message, MsgBoxStyle.Critical, "Unable to upload XML")
        End Try

    End Function



    Function requestData(vURL As String) As String
        'HTTP variable
        Dim myHTTP As New MSXML.XMLHTTPRequest
        'Dim doc As XmlDocument = New XmlDocument()
        Dim strReturn As String
        'uploadData = False
        Try
            'doc.Load(vFile)
            With myHTTP
                .open("Post", vURL, False)
                .setRequestHeader("Content-Type", "text/xml")
                .send("") '+64
                strReturn = .responseText
                'If strReturn <> """Successful""" Then
                '    MsgBox(strReturn)
                'End If
            End With
            Return strReturn
        Catch ex As Exception

            MsgBox("Unable to upload XML!!!" & vbCrLf & _
                "Because " & ex.Message, MsgBoxStyle.Critical, "Unable to upload XML")
            Return "Error"
        End Try

    End Function

    Sub initialControl()
        btnDatabase.Text = "&Connect Database" : btnDatabase.Enabled = True
        btnImport.Text = "&Start Import data" : btnImport.Enabled = False
        tssDatabase.Text = "Program ready"
        lblLastDate.Text = objInI.GetString("Last execution", "date", "")
        lblFrom.Text = lblLastDate.Text
        lblTo.Text = getDateTo(lblLastDate.Text)
        gbImport.Text = "Import Details"
        lblPeriod.Text = objInI.GetString("import", "range", "")
        lblLoop.Text = objInI.GetString("import", "interval", "")
        lblNextRun.Text = Now
        vServiceURL = objInI.GetString("service", "url", "")
    End Sub

    Function getDateTo(vDateFrom As String) As String
        Dim vRange As String = objInI.GetString("import", "range", "")
        Dim date2 As Date = vDateFrom
        getDateTo = date2.AddHours(Val(vRange))
    End Function


    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        If MsgBox("Are you sure to close program", vbQuestion + vbYesNo, "Confrim close program") = vbYes Then
            On Error Resume Next
            objFits.disconnect()
            Me.Close()
        End If

    End Sub


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        objInI = New clsINI(Application.StartupPath & "\import.ini")
        Timer1.Interval = (Val(objInI.GetString("import", "interval", ""))) * 1000 * 60
        Timer1.Enabled = False

        vWorkingDir = objInI.GetString("path", "working dir", "")
        If vWorkingDir = "" Then
            vWorkingDir = Application.StartupPath & "\"
        End If



        initialControl()
        Me.Text = Me.Text + " (Target : " + vServiceURL + ")"
    End Sub

    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        'First import then using Timer.
        If btnImport.Text = "&Start Import data" Then
            ExportData()
            Timer1.Enabled = True
            btnImport.Text = "&Stop Import data"
            Timer2.Enabled = True
            '--Update From/To date

        Else
            Timer1.Enabled = False
            Timer2.Enabled = False
            btnImport.Text = "&Start Import data"
        End If

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Enabled = False
        Timer2.Enabled = False
        ExportData()
        lblNextRun.Text = Now.AddMinutes(Val(objInI.GetString("import", "interval", "")))
        Timer1.Enabled = True
        Timer2.Enabled = True
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        tssStatus.Text = "Next run (remaining time) -->" & Print_Remaining_Time(CDate(lblNextRun.Text))
    End Sub

    Public Function Print_Remaining_Time(EndTime As DateTime)
        'If EndTime.ToString = "01/01/0001 0:00:00" Then
        '    EndTime = Now
        '    EndTime = EndTime.AddMilliseconds(Time_Out - 1000)
        'End If
        Dim RemainingTime As TimeSpan
        RemainingTime = Now().Subtract(EndTime)
        Return String.Format("{0:00}:{1:00}:{2:00}", CInt(Math.Floor(RemainingTime.TotalHours)) Mod 60, CInt(Math.Floor(RemainingTime.TotalMinutes)) Mod 60, CInt(Math.Floor(RemainingTime.TotalSeconds)) Mod 60).Replace("-", "")
    End Function

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        uploadData("C:\Users\chutchais\Documents\Visual Studio 2013\Projects\test.xml")
    End Sub
End Class
