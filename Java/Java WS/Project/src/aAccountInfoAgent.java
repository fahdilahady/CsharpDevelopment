import java.awt.BorderLayout;
import java.awt.Dimension;
import java.awt.GridLayout;

import javax.swing.JDesktopPane;
import javax.swing.JFrame;
import javax.swing.JInternalFrame;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JTextField;


public class aAccountInfoAgent extends JFrame{
	
	JDesktopPane dp = new JDesktopPane();
	
	JInternalFrame ifAccountMember = new JInternalFrame();

	JLabel JudulAccountInformation = new JLabel("Account Information");
	JLabel MemberID = new JLabel("Member ID");
	JLabel Name = new JLabel("Name");
	JLabel AccountMember = new JLabel("Account Member");
	JLabel Balance = new JLabel("Balance"); 
	
	JPanel pnlJudul = new JPanel();
	JPanel pnlMiddle = new JPanel(new GridLayout(8, 2));
	JPanel pnlButton = new JPanel();
	JPanel pnlPosisi = new JPanel(new BorderLayout());
	
	public aAccountInfoAgent(){
		
		pnlJudul.add(JudulAccountInformation);
		pnlMiddle.add(MemberID);
		pnlMiddle.add(Name);
		pnlMiddle.add(AccountMember);
		pnlMiddle.add(Balance);
		
		pnlPosisi.add(pnlJudul, BorderLayout.NORTH);
		pnlPosisi.add(pnlMiddle, BorderLayout.CENTER);
		pnlPosisi.add(pnlButton, BorderLayout.SOUTH);
		
		ifAccountMember.add(pnlPosisi, BorderLayout.CENTER);
		
		dp.add(ifAccountMember);
		
		add(dp, BorderLayout.CENTER);
		dp.setVisible(true);
		ifAccountMember.setVisible(true);
		ifAccountMember.setClosable(true);
		ifAccountMember.setSize(new Dimension(350, 200));
		setVisible(true);
		
		setExtendedState(JFrame.MAXIMIZED_BOTH);
	}
	public static void main(String[] args) {
	
		new aAccountInfoAgent();

	}

}
