import java.util.Scanner;


public class aCar extends aVehicle {
	int MaxFuelCapacity = 35;
	int InitialDistance = 15;
	public aCar()
	{
		Type = tVehicleType.CAR;
	}
	public boolean isEmptyFuel(){
		
		return fuelInTank == 0 || super.isEmptyFuel();
	}
	public String getMyBrand(Scanner curScan){
		do{
		System.out.println("Input your car's brand ['BMX' | 'Toyoda' | 'Pissan']:");
		Brand = curScan.nextLine();
		}while (!Brand.equals("BMX")||!Brand.equals("Toyoda")||!Brand.equals("Pissan"));
		
		return Brand;
		
	}
}
