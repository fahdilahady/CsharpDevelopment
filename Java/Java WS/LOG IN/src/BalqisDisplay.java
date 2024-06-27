import javax.swing.*;
import java.awt.GridLayout;
import java.awt.BorderLayout;
public class BalqisDisplay extends JFrame {
	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;

	public BalqisDisplay(){
		setLayout (new GridLayout(3, 2, 5, 4));
		add(new JLabel("USER"));
		add(new JTextField(8));
		add(new JLabel("PASSWORD"));
		add(new JPasswordField(8));
		add(new JButton("log in"),BorderLayout.SOUTH);
		add(new JButton("Cancel"),BorderLayout.SOUTH);
	}
	
}

