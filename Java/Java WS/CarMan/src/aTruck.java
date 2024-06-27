import java.util.Scanner;


public class aTruck extends aVehicle {
	int MaxFuelCapacity = 70;
	int InitialDistance = 100;
	
	public aTruck(){
		Type = tVehicleType.TRUCK; 
	}
	
	public String getMyBrand(Scanner curScan){
		do{
		System.out.println("Input your Truck's brand ['Wercedes' | 'Polpo' | 'Lord']:");
		Brand = curScan.nextLine();
		}while (Brand!="Wercedes"||Brand!="Polpo"||Brand != "Lord");
		
		return Brand;
		
	}
}
