import java.util.Scanner;
public class aVehicleManager {

	/**
	 * @param args
	 */
	static aPersonAsUser CurrentUser;
	static Scanner myScanner;
	
	private static void Clear(){
		for(int i = 0; i < 30; i++){
			System.out.println("");
		}
	}
	public static void main(String[] args) {
		// TODO Auto-generated method stub
		String name="";
		int ID=0;
		int selectedMenu=0;
		aVehicle CurVehicle = null;
		if (myScanner == null){
			myScanner  = new Scanner(System.in);
		} 
		
		if (CurrentUser == null){
			name = GetUserName();
			if (ID ==0){
				ID =(int)(Math.random()*1000);
			}
			
			CurVehicle = GetVehicle();
			CurrentUser = new aPersonAsUser(name,ID,CurVehicle);
		}
		do{
		selectedMenu= GetSelectedMenu();
		
		switch (selectedMenu)
		{
			case 1: {
				// Menu Drive
				if (CurVehicle.isEmptyFuel()){
					System.out.println("Fuel is empty, please refill...");
					break;
				}else{
					int distance =0;
					while (distance < 1){
						distance = myScanner.nextInt();}
					
					if (CurVehicle.isFuelEnoughToDrive(CurVehicle.fuelInTank, distance)){
						switch (CurVehicle.Type){
							case TRUCK:{
								System.out.println("do you want to carry item [Y | N]");
								String isCarryGoods = myScanner.nextLine(); isCarryGoods.toUpperCase(); 
								if (isCarryGoods.equals("Y")){
									
									}
								}
							case CAR:{
								System.out.println("your car is ready...");
								break;
								}
							default:
						}
					}
					
				}
			}
			case 2:{
				CurVehicle.RefillFuel();
			}
			case 3:{
				
			}
			case 4:{
				
			}
			case 5:{
				
			}
			default:
		}
		Clear();
		}while(selectedMenu <5 );
	
	}
	
	private static aVehicle GetVehicle(){
		
		String userIn;
		boolean isValid;
		
		do{
		System.out.println("input your vehicle [Car | Truck]");
		userIn = myScanner.nextLine();
		userIn = userIn.toUpperCase(); 
		isValid = userIn.equals("CAR") || userIn.equals("TRUCK");
		}
		while (isValid== false );
		
		switch (userIn)
		{
		case  "CAR":{
		
			aCar curCar = new aCar();
			
			curCar.getMyBrand(myScanner);
			
			return curCar;}
		
		case "TRUCK":
		{
			aTruck curTruck = new aTruck();
			
			curTruck.getMyBrand(myScanner);
			
			return curTruck;}
		
		default :return null;
		}
			
		
	}
	
	private static String Menus(int byIndex){
		switch (byIndex){
			case 1: return "Drive";
			case 2: return "Refill gas";
			case 3: return "Check vehicle";
			case 4: return "Change vehicle";
			default : return "Invalid options";
		}
	}

	private static int GetSelectedMenu(){
		int _result;
		do{
			System.out.println("Welcome, "+CurrentUser.Name);
		
			System.out.println("1. " + Menus(1));
			System.out.println("2. " + Menus(2));
			System.out.println("3. " + Menus(3));
			System.out.println("4. " + Menus(4));
			System.out.println("5. Exit");
			_result = myScanner.nextInt();
		}while(_result < 1 || _result >5);
		return _result;

	}
	
	public static String GetUserName(){
		String Name;
		
		System.out.println("Welcome,");
		do{
		System.out.print("Please enter your name [5 - 25 chars]: ");
		Name = myScanner.nextLine();
		}
		while(Name.length() < 5 || Name.length() > 25);

		return Name;

	}
	
}
