import javax.swing.*;
import java.awt.BorderLayout;
import java.awt.GridLayout;
import java.awt.event.*;


public class aRegisterFormAgent extends JFrame implements ActionListener{
	/** this is form agent to create a user account into application and database
	 * 
	 */
	JTextField MemberIDInput, UserNameInput,AccountNumberInput,  BalanceInput; 
	JPasswordField PinInput;
	JTextArea HomeAddressInput;
	JLabel TitleLbl, MemberIDLbl, NameLbl, PinLbl , AddressLbl,  AccountNumberLbl, BalanceLbl, DateOfBirthLbl, GenderLbl;
	
	JPanel TitlePnl, MidPnl, BtnPnl, GenderPnl, DateBirthPnl, PosPnl;
	JInternalFrame myIntFrame; JDesktopPane myDP;
	
	
	JRadioButton MaleBtn, FemaleBtn;
	
    ButtonGroup GenderOpt;
   

	JButton OkBtn, ResetBtn, Cancel;
	
	private static final long serialVersionUID = 3380567188327594568L;
	
	
	protected void initAgents(){
		//Buttons Initiate
		OkBtn = new JButton("OK"); ResetBtn =new JButton("Reset"); Cancel =new JButton("Cancel");
		//TextField Initiate
		MemberIDInput = new JTextField(); UserNameInput= new JTextField(); PinInput= new JPasswordField();HomeAddressInput= new JTextArea();AccountNumberInput= new JTextField();  
		BalanceInput= new JTextField();
		
		//Labels Initiate
		TitleLbl =new JLabel("Register");
		MemberIDLbl=new JLabel("Member ID");
		NameLbl=new JLabel("Name");
		PinLbl =new JLabel("Pin");AddressLbl=new JLabel("Address");AccountNumberLbl=new JLabel("AccountNumber");
		BalanceLbl=new JLabel("Balance");DateOfBirthLbl=new JLabel("Date Of Birth");GenderLbl=new JLabel("Gender");
		
		//Gender Radio Button 
		 MaleBtn= new JRadioButton("Male");
		 MaleBtn.setActionCommand("Male");
		 MaleBtn.setSelected(true);
		 
		 FemaleBtn = new JRadioButton("Female");
		 FemaleBtn.setActionCommand("Female");
		 
		 GenderOpt = new ButtonGroup(); 
		 GenderOpt.add(MaleBtn);
		 GenderOpt.add(FemaleBtn);

		 
		 TitlePnl= new JPanel(); MidPnl= new JPanel(new GridLayout(8, 2)); BtnPnl= new JPanel();
		  
		 GenderPnl= new JPanel();DateBirthPnl= new JPanel();
		 PosPnl= new JPanel(new BorderLayout());
		 
		 JComboBox<Integer> cbDate = new JComboBox<Integer>();
		 for (int i =1; i <= 31; i++) {
			 cbDate.addItem(i);	
		 }
		 JComboBox<Integer> cbMonth = new JComboBox<Integer>();
		 for (int i = 1; i <= 12; i++) {
			 cbMonth.addItem(i);
		 }
		 JComboBox<Integer> cbYear = new JComboBox<Integer>();
		 for (int i = 1980; i <= 2016; i++) {
			 cbYear.addItem(i);
		 }
		 DateBirthPnl.add(cbDate);DateBirthPnl.add(cbMonth);DateBirthPnl.add(cbYear);
		 
		 myIntFrame = new JInternalFrame(); myDP = new JDesktopPane();
		 
	}
	protected void placeAgents() {
		
		GenderPnl.add(MaleBtn);GenderPnl.add(FemaleBtn);
		
		TitlePnl.add(TitleLbl);
		
		MidPnl.add(MemberIDLbl);MidPnl.add(MemberIDInput);
		MidPnl.add(NameLbl);MidPnl.add(UserNameInput);
		MidPnl.add(GenderLbl);MidPnl.add(GenderPnl);
		MidPnl.add(DateOfBirthLbl);MidPnl.add(DateBirthPnl);
		MidPnl.add(AddressLbl); MidPnl.add(HomeAddressInput);
		MidPnl.add(AccountNumberLbl);MidPnl.add(AccountNumberInput);
		MidPnl.add(BalanceLbl); MidPnl.add(BalanceInput);
		MidPnl.add(PinLbl);MidPnl.add(PinInput);
		
		BtnPnl.add(ResetBtn);
		BtnPnl.add(OkBtn);
		BtnPnl.add(Cancel);
		
		PosPnl.add(TitlePnl, BorderLayout.NORTH);
		PosPnl.add(MidPnl, BorderLayout.CENTER);
		PosPnl.add(BtnPnl, BorderLayout.SOUTH);
		
		add(PosPnl, BorderLayout.CENTER);
		//myDP.add(myIntFrame);
		
		//add(myDP, BorderLayout.CENTER);
		//myDP.setVisible(true); myIntFrame.setVisible(true);
		//myIntFrame.pack();
		
		
		//setExtendedState(JFrame.MAXIMIZED_BOTH);
	}
	
	public aRegisterFormAgent() {
		// TODO Auto-generated constructor stub
		initAgents(); placeAgents();
		OkBtn.addActionListener(this);ResetBtn.addActionListener(this);Cancel.addActionListener(this);
	}
	
	
	protected void  acceptUserInfo() {
		aUser theUser = new aUser();
		
		//theUser.SetName
		if (theUser.isAccepted()== true){
			//Notify user by PopUp Message , Registration Completed
		}
	}

	@Override
	public void actionPerformed(ActionEvent e) {
		// TODO Auto-generated method stub
		
		if(e.getSource()== OkBtn){
			//Submit to mdb driver
		}else if (e.getSource() == ResetBtn){
			// ClearDisplay
		}else if (e.getSource() == Cancel){
			//Action Close
		}
	}
}
