using System;
using System.Collections.Generic;
using System.Linq;

namespace ParkingSystem
{
    public class ParkingLot
    {
        private int totalSlots;
        private Dictionary<int, Vehicle> parkingSlots;

        public ParkingLot(int totalSlots)
        {
            this.totalSlots = totalSlots;
            parkingSlots = new Dictionary<int, Vehicle>();
            Console.WriteLine($"Created a parking lot with {totalSlots} slots");
        }

        public void ParkVehicle(string registrationNo, string color, string type)
        {
            if (IsParkingFull())
            {
                Console.WriteLine("Sorry, parking lot is full");
                return;
            }

            int slotNumber = GetNextAvailableSlot();
            parkingSlots[slotNumber] = new Vehicle(registrationNo, color, type);
            Console.WriteLine($"Allocated slot number: {slotNumber}");
        }

        public void Leave(int slotNumber)
        {
            if (!parkingSlots.ContainsKey(slotNumber))
            {
                Console.WriteLine($"Slot number {slotNumber} is already free");
                return;
            }

            parkingSlots.Remove(slotNumber);
            Console.WriteLine($"Slot number {slotNumber} is free");
        }

        public void Status()
        {
            Console.WriteLine("Slot\tNo.\tType\tRegistration No\tColour");
            foreach (var entry in parkingSlots.OrderBy(e => e.Key))
            {
                Console.WriteLine($"{entry.Key}\t{entry.Value.RegistrationNo}\t{entry.Value.Type}\t{entry.Value.Color}");
            }
        }

        public void TypeOfVehicles(string type)
        {
            int count = parkingSlots.Count(v => v.Value.Type.ToLower() == type.ToLower());
            Console.WriteLine(count);
        }

        public void RegistrationNumbersForVehiclesWithOddPlate()
        {
            var oddPlates = parkingSlots.Where(v => IsOddPlate(v.Value.RegistrationNo)).Select(v => v.Value.RegistrationNo);
            Console.WriteLine(string.Join(", ", oddPlates));
        }

        public void RegistrationNumbersForVehiclesWithEvenPlate()
        {
            var evenPlates = parkingSlots.Where(v => !IsOddPlate(v.Value.RegistrationNo)).Select(v => v.Value.RegistrationNo);
            Console.WriteLine(string.Join(", ", evenPlates));
        }

        public void RegistrationNumbersForVehiclesWithColour(string color)
        {
            var vehicles = parkingSlots.Where(v => v.Value.Color.ToLower() == color.ToLower()).Select(v => v.Value.RegistrationNo);
            Console.WriteLine(string.Join(", ", vehicles));
        }

        public void SlotNumbersForVehiclesWithColour(string color)
        {
            var slots = parkingSlots.Where(v => v.Value.Color.ToLower() == color.ToLower()).Select(v => v.Key);
            Console.WriteLine(string.Join(", ", slots));
        }

        public void SlotNumberForRegistrationNumber(string registrationNo)
        {
            var slot = parkingSlots.FirstOrDefault(v => v.Value.RegistrationNo == registrationNo);
            if (slot.Equals(default(KeyValuePair<int, Vehicle>)))
                Console.WriteLine("Not found");
            else
                Console.WriteLine(slot.Key);
        }

        private bool IsParkingFull()
        {
            return parkingSlots.Count >= totalSlots;
        }

        private int GetNextAvailableSlot()
        {
            for (int i = 1; i <= totalSlots; i++)
            {
                if (!parkingSlots.ContainsKey(i))
                    return i;
            }
            return -1; 
        }

        private bool IsOddPlate(string plate)
        {
            char lastChar = plate[plate.Length - 1];
            return (lastChar - '0') % 2 != 0;
        }
    }

    public class Vehicle
    {
        public string RegistrationNo { get; }
        public string Color { get; }
        public string Type { get; }

        public Vehicle(string registrationNo, string color, string type)
        {
            RegistrationNo = registrationNo;
            Color = color;
            Type = type;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ParkingLot parkingLot = null;

            while (true)
            {
                string input = Console.ReadLine();
                string[] inputs = input.Split(' ');

                switch (inputs[0])
                {
                    case "create_parking_lot":
                        int totalSlots = int.Parse(inputs[1]);
                        parkingLot = new ParkingLot(totalSlots);
                        break;

                    case "park":
                        parkingLot.ParkVehicle(inputs[1], inputs[2], inputs[3]);
                        break;

                    case "leave":
                        int slotNumber = int.Parse(inputs[1]);
                        parkingLot.Leave(slotNumber);
                        break;

                    case "status":
                        parkingLot.Status();
                        break;

                    case "type_of_vehicles":
                        parkingLot.TypeOfVehicles(inputs[1]);
                        break;

                    case "registration_numbers_for_vehicles_with_odd_plate":
                        parkingLot.RegistrationNumbersForVehiclesWithOddPlate();
                        break;

                    case "registration_numbers_for_vehicles_with_even_plate":
                        parkingLot.RegistrationNumbersForVehiclesWithEvenPlate();
                        break;

                    case "registration_numbers_for_vehicles_with_colour":
                        parkingLot.RegistrationNumbersForVehiclesWithColour(inputs[1]);
                        break;

                    case "slot_numbers_for_vehicles_with_colour":
                        parkingLot.SlotNumbersForVehiclesWithColour(inputs[1]);
                        break;

                    case "slot_number_for_registration_number":
                        parkingLot.SlotNumberForRegistrationNumber(inputs[1]);
                        break;

                    case "exit":
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Invalid command");
                        break;
                }
            }
        }
    }
}
