import java.awt.*;


import javax.swing.JButton;
import javax.swing.JDesktopPane;
import javax.swing.JFrame;
import javax.swing.JInternalFrame;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JTextField;


public class aTrFormAgent extends JFrame{

	private static final long serialVersionUID = -11734823501301284L;

	/**
	 * 
	 */


	JDesktopPane dp = new JDesktopPane();
	
	JInternalFrame iftransfer = new JInternalFrame();
	
	JLabel judulTransfer = new JLabel("Transfer");
	JLabel Amount = new JLabel("Amount(Rp)");
	JLabel ReceiverAccounttNumber = new JLabel("Receiver Account Number");
	
	JButton Transfer = new JButton("Transfer");
	
	JTextField txtAmount = new JTextField();
	JTextField txtReceiverAccountNumber = new JTextField();
	
	JPanel pnlJudul = new JPanel();
	JPanel pnlMiddle = new JPanel(new GridLayout(2, 2));
	JPanel pnlButton = new JPanel();
	JPanel pnlPosisi = new JPanel(new BorderLayout());
	
	public aTrFormAgent(){
	
		pnlButton.add(Transfer);
		
		pnlJudul.add(judulTransfer);
		pnlMiddle.add(Amount);
		pnlMiddle.add(txtAmount);
		pnlMiddle.add(ReceiverAccounttNumber);
		pnlMiddle.add(txtReceiverAccountNumber);
		
		pnlPosisi.add(pnlJudul, BorderLayout.NORTH);
		pnlPosisi.add(pnlMiddle, BorderLayout.CENTER);
		pnlPosisi.add(pnlButton, BorderLayout.SOUTH);
		
		iftransfer.add(pnlPosisi, BorderLayout.CENTER);
		
		dp.add(iftransfer);
		
		add(dp, BorderLayout.CENTER);
		dp.setVisible(true);
		iftransfer.setVisible(true);
		iftransfer.setClosable(true);
		iftransfer.setSize(new Dimension(350, 150));
		setVisible(true);
		
		
		setExtendedState(JFrame.MAXIMIZED_BOTH);
	}
	public static void main(String[] args) {
	
		new aTrFormAgent();

	}

}
