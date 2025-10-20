<%@ Page Title="JokerDicePoker" Language="C#" MasterPageFile="~/Gamez/GamesMaster.master" AutoEventWireup="true" CodeBehind="JokerDice.aspx.cs" Inherits="Area23.At.Mono.Gamez.JokerDice" %>

<asp:Content ID="ContentEncodeHead" ContentPlaceHolderID="EncodeHead" runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>JokerDicePoker</title>
    <link rel="stylesheet" href="../res/css/area23.at.mono.css" />
</asp:Content>
<asp:Content ID="ContentEncodeBody" ContentPlaceHolderID="EncodeBody" runat="server" ClientIDMode="Static">     
    <form id="Form_JokerDice" runat="server">
        <div>            
        <asp:Table ID="TablePC" runat="server">
            <asp:TableRow ID="TableRowP" runat="server">
                <asp:TableCell ID="TableCellP0" runat="server" Width="12%" Style="vertical-align: text-top;">
                    <asp:Literal ID="Literal_P0" runat="server" Text="Player" />
                </asp:TableCell>
                <asp:TableCell ID="TableCellP1" runat="server" Width="12%" Style="vertical-align: text-top;">
                    <asp:ImageButton ID="ImageP1" runat="server" ClientIDMode="Static" ImageUrl="~/res/img/symbol/Joker.png" Width="122px" Height="122px" OnClick="ImageP_Click" />
                </asp:TableCell>
                <asp:TableCell ID="TableCellP2" runat="server" Width="12%" Style="vertical-align: text-top;">
                    <asp:ImageButton ID="ImageP2" runat="server" ClientIDMode="Static" ImageUrl="~/res/img/symbol/Jack.png" Width="122px" Height="122px" OnClick="ImageP_Click" />
                </asp:TableCell>
                <asp:TableCell ID="TableCellP3" runat="server" Width="12%" Style="vertical-align: text-top;">
                    <asp:ImageButton ID="ImageP3" runat="server" ClientIDMode="Static" ImageUrl="~/res/img/symbol/Queen.png" Width="122px" Height="122px" OnClick="ImageP_Click" />
                </asp:TableCell>
                <asp:TableCell ID="TableCellP4" runat="server" Width="12%" Style="vertical-align: text-top;">
                    <asp:ImageButton ID="ImageP4" runat="server" ClientIDMode="Static" ImageUrl="~/res/img/symbol/King.png" Width="122px" Height="122px" OnClick="ImageP_Click"  />
                </asp:TableCell>
                <asp:TableCell ID="TableCellP5" runat="server" Width="12%" Style="vertical-align: text-top;">
                    <asp:ImageButton ID="ImageP5" runat="server" ClientIDMode="Static" ImageUrl="~/res/img/symbol/Ace.png" Width="122px" Height="122px" OnClick="ImageP_Click" />
                </asp:TableCell>
                <asp:TableCell ID="TableCellP6" runat="server" Width="12%" Style="vertical-align: text-top;">
                    <asp:ImageButton ID="ImageButton_DiceCup" runat="server" ClientIDMode="Static" ImageUrl="~/res/img/symbol/DiceCup.png" Width="122px" Height="122px" OnClick="ImageButton_DiceCup_Click" ToolTip="Click to roll the dice!" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRowC" runat="server" visible="false">
                <asp:TableCell ID="TableCellC0" runat="server" Width="12%" Style="vertical-align: text-top;">
                    <asp:Literal ID="Literal_C0" runat="server" Text="Computer" />
                </asp:TableCell>
                <asp:TableCell ID="TableCellC1" runat="server" Width="12%" Style="vertical-align: text-top;">
                    <asp:Image ID="ImageC1" runat="server" ImageUrl="~/res/img/symbol/EmptyDice.png" Width="122px" Height="122px" />
                </asp:TableCell>
                <asp:TableCell ID="TableCellC2" runat="server" Width="12%" Style="vertical-align: text-top;">
                    <asp:Image ID="ImageC2" runat="server"  ImageUrl="~/res/img/symbol/EmptyDice.png" Width="122px" Height="122px" />
                </asp:TableCell>
                <asp:TableCell ID="TableCellC3" runat="server" Width="12%" Style="vertical-align: text-top;">
                    <asp:Image ID="ImageC3" runat="server"  ImageUrl="~/res/img/symbol/EmptyDice.png" Width="122px" Height="122px" />
                </asp:TableCell>
                <asp:TableCell ID="TableCellC4" runat="server" Width="12%" Style="vertical-align: text-top;">
                    <asp:Image ID="ImageC4" runat="server"  ImageUrl="~/res/img/symbol/EmptyDice.png" Width="122px" Height="122px" />
                </asp:TableCell>
                <asp:TableCell ID="TableCellC5" runat="server" Width="12%" Style="vertical-align: text-top;">
                    <asp:Image ID="ImageC5" runat="server" ImageUrl="~/res/img/symbol/EmptyDice.png" Width="122px" Height="122px" />
                </asp:TableCell>
                <asp:TableCell ID="TableCellC6" runat="server" Width="12%" Style="vertical-align: text-top;">
                    <asp:Literal ID="Literal_End" runat="server" Text="" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <hr />
        <asp:Literal ID="Literal_Action" runat="server" ClientIDMode="Static" Text="Click on dice cup above" />
        <asp:Table ID="TablePoints" runat="server">
            <asp:TableHeaderRow ID="TableHeaderRowPoints" runat="server">
                <asp:TableCell ID="TableHeaderCell" runat="server">&nbsp;</asp:TableCell>
                <asp:TableCell ID="TableHeaderCellPlayer" runat="server"><b>player</b></asp:TableCell>
                <asp:TableCell ID="TableHeaderCellAction" runat="server"><b>action</b></asp:TableCell>
                <asp:TableCell ID="TableHeaderCellComputer" runat="server" Visible="false"><b>computer</b></asp:TableCell>
            </asp:TableHeaderRow>
            <asp:TableRow ID="TableRowGrande" runat="server">
                <asp:TableCell ID="TableCellGrande" runat="server" style="text-decoration: none">grande</asp:TableCell>
                <asp:TableCell ID="TableCellGrandePlayer" runat="server"><asp:CheckBox ID="CheckBoxGrande" runat="server" AutoPostBack="true" ClientIDMode="Static" Enabled="false" OnCheckedChanged="PokerCheckBox_Changed" /></asp:TableCell>
                <asp:TableCell ID="TableCellGrandeAction" runat="server"><asp:ImageButton ID="ImageButtonGrande" runat="server" ClientIDMode="Static" ImageUrl="~/res/img/symbol/pencila.png" OnClick="ImageButton_Pencil_Click" /></asp:TableCell>
                <asp:TableCell ID="TableCellGrandeComputer" runat="server" Visible="false">0</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRowPoker" runat="server">
                <asp:TableCell ID="TableCellPoker" runat="server" style="text-decoration: none">poker</asp:TableCell>
                <asp:TableCell ID="TableCellPokerPlayer" runat="server"><asp:CheckBox ID="CheckBoxPoker" runat="server" ClientIDMode="Static" AutoPostBack="true" Enabled="false" OnCheckedChanged="PokerCheckBox_Changed" /></asp:TableCell>
                <asp:TableCell ID="TableCellPokerAction" runat="server"><asp:ImageButton ID="ImageButtonPoker" runat="server" ClientIDMode="Static" ImageUrl="~/res/img/symbol/pencila.png" OnClick="ImageButton_Pencil_Click" /></asp:TableCell>
                <asp:TableCell ID="TableCellPokerComputer" runat="server" Visible="false">0</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRowFullHouse" runat="server">
                <asp:TableCell ID="TableCellFullHouse" runat="server" style="text-decoration: none">FullHouse</asp:TableCell>
                <asp:TableCell ID="TableCellFullHousePlayer" runat="server"><asp:CheckBox ID="CheckBoxFullHouse" runat="server" ClientIDMode="Static" AutoPostBack="true" Enabled="false" OnCheckedChanged="PokerCheckBox_Changed" /></asp:TableCell>
                <asp:TableCell ID="TableCellFullHouseAction" runat="server"><asp:ImageButton ID="ImageButtonFullHouse" runat="server" ClientIDMode="Static" ImageUrl="~/res/img/symbol/pencila.png" OnClick="ImageButton_Pencil_Click" /></asp:TableCell>
                <asp:TableCell ID="TableCellFullHouseComputer" runat="server" Visible="false">0</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRowStraight" runat="server">
                <asp:TableCell ID="TableCellStraight" runat="server" style="text-decoration: none">Straight</asp:TableCell>
                <asp:TableCell ID="TableCellStraightPlayer" runat="server"><asp:CheckBox ID="CheckBoxStraight" runat="server" ClientIDMode="Static" AutoPostBack="true" Enabled="false" OnCheckedChanged="PokerCheckBox_Changed" /></asp:TableCell>
                <asp:TableCell ID="TableCellStraightAction" runat="server"><asp:ImageButton ID="ImageButtonStraight" runat="server" ClientIDMode="Static" ImageUrl="~/res/img/symbol/pencila.png" OnClick="ImageButton_Pencil_Click" /></asp:TableCell>
                <asp:TableCell ID="TableCellStraightComputer" runat="server" Visible="false">0</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRowTriple" runat="server">
                <asp:TableCell ID="TableCellTriple" runat="server" style="text-decoration: none">Triple</asp:TableCell>
                <asp:TableCell ID="TableCellTriplePlayer" runat="server"><asp:CheckBox ID="CheckBoxTriple" runat="server" ClientIDMode="Static" AutoPostBack="true" Enabled="false" OnCheckedChanged="PokerCheckBox_Changed" /></asp:TableCell>
                <asp:TableCell ID="TableCellTripleAction" runat="server">    <asp:ImageButton ID="ImageButtonTriple" runat="server" ClientIDMode="Static" ImageUrl="~/res/img/symbol/pencila.png" OnClick="ImageButton_Pencil_Click" /></asp:TableCell>
                <asp:TableCell ID="TableCellTripleComputer" runat="server" Visible="false">0</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRowTwoPairs" runat="server">
                <asp:TableCell ID="TableCellTwoPairs" runat="server"><span id="SpanTwoPairs" runat="server">TwoPairs</span></asp:TableCell>
                <asp:TableCell ID="TableCellTwoPairsPlayer" runat="server"><asp:CheckBox ID="CheckBoxTwoPairs" runat="server" ClientIDMode="Static" AutoPostBack="true" Enabled="false" OnCheckedChanged="PokerCheckBox_Changed" /></asp:TableCell>
                <asp:TableCell ID="TableCellTwoPairsAction" runat="server"><asp:ImageButton ID="ImageButtonTwoPairs" runat="server" ClientIDMode="Static" ImageUrl="~/res/img/symbol/pencila.png" OnClick="ImageButton_Pencil_Click" /></asp:TableCell>
                <asp:TableCell ID="TableCellTwoPairsComputer" runat="server"  Visible="false">0</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRowPair" runat="server">
                <asp:TableCell ID="TableCellPair" runat="server"><span id="SpanPair" runat="server">Pair</span></asp:TableCell>
                <asp:TableCell ID="TableCellPairPlayer" runat="server"><asp:CheckBox ID="CheckBoxPair" runat="server" ClientIDMode="Static" AutoPostBack="true" Enabled="false" OnCheckedChanged="PokerCheckBox_Changed" /></asp:TableCell>
                <asp:TableCell ID="TableCellPairAction" runat="server"><asp:ImageButton ID="ImageButtonPair" runat="server" ClientIDMode="Static" ImageUrl="~/res/img/symbol/pencila.png" OnClick="ImageButton_Pencil_Click" /></asp:TableCell>
                <asp:TableCell ID="TableCellPairComputer" runat="server" Visible="false">0</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRowBust" runat="server">                
                <asp:TableCell ID="TableCellBust" runat="server"><span id="SpanBust" runat="server">Bust</span></asp:TableCell>
                <asp:TableCell ID="TableCellBustPlayer" runat="server"><asp:CheckBox ID="CheckBoxBust" runat="server" ClientIDMode="Static" AutoPostBack="true" Enabled="false" OnCheckedChanged="PokerCheckBox_Changed"  /></asp:TableCell>
                <asp:TableCell ID="TableCellBustAction" runat="server"><asp:ImageButton ID="ImageButtonBust" runat="server" ClientIDMode="Static" ImageUrl="~/res/img/symbol/pencila.png" OnClick="ImageButton_Pencil_Click" /></asp:TableCell>                
                <asp:TableCell ID="TableCellBustComputer" runat="server"  Visible="false">0</asp:TableCell>
            </asp:TableRow>
        </asp:Table>
            <div style="width:80%; min-width: 700px; height:156px;"><img src="../res/img/symbol/Dice_Poker_Header.png" border="0" alt="Dice Poker" /></div>
        </div>
    </form>
</asp:Content>
