import javax.swing.*;


public class Main {

	public static void main(String[] args) {
		getUserAuthenticate();
		
	}
protected static void getUserAuthenticate(){
	aLoginAgent Loginfrm = new aLoginAgent ();
	Loginfrm.setTitle("LOG IN");
	Loginfrm.setSize(450, 150);
	Loginfrm.setLocationRelativeTo(null);
	Loginfrm.setDefaultCloseOperation (JFrame.EXIT_ON_CLOSE);
	Loginfrm.setVisible(true);

}
}

