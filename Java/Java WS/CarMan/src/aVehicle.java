import java.util.Scanner;
public class aVehicle {
	
	tVehicleType Type;
	String Brand;
	int fuelInTank;
	int MaxFuelCapacity = 0;
	int InitialDistance =0;
	public String getMyBrand(Scanner curScan){
		return "";
	}
	public boolean isEmptyFuel(){
		return fuelInTank == 0;
	}
	public void RefillFuel(){
		fuelInTank = MaxFuelCapacity;
	}
	
	public int MaxDistance(int curFuel){
		return curFuel * InitialDistance;
	}
	
	public boolean isFuelEnoughToDrive(int curFuel, int distance)
	{
		return curFuel * distance <=   MaxDistance(curFuel);
	}
	
	public tVehicleType GetVehicleType(){
		//must be overridden
		return Type;
	}
}
