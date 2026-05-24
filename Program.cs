using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Project
{
	class Program
	{
		// Full screen console ke liye
		[DllImport("kernel32.dll", ExactSpelling = true)]
		private static extern IntPtr GetConsoleWindow();
		private static IntPtr ThisConsole = GetConsoleWindow();
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
		private const int MAXIMIZE = 3;

		// File paths constants
		private const string APPOINTMENTS_DIR = "Appointments";
		private const string DOCTORS_DIR = "doctors";
		private const string DOCTORS_LIST_FILE = "DoctorsList.txt";
		private const string ALL_APPOINTMENTS_FILE = APPOINTMENTS_DIR + "/All_Appointments.txt";
		private const string DONORS_RECEIPT_FILE = "receipt.txt";
		private const string REQUESTS_FILE = "Request.txt";

		static void Main(string[] args)
		{
			// Create necessary directories
			Directory.CreateDirectory(APPOINTMENTS_DIR);
			Directory.CreateDirectory(DOCTORS_DIR);

			// Ensure doctors files exist (create dummy if not exists)
			EnsureFileExists(DOCTORS_LIST_FILE, "1 = Dr. Ali (Cardiologist)\n2 = Dr. Sara (Neurologist)\n3 = Dr. Ahmed (Pediatrician)");
			EnsureFileExists(DOCTORS_DIR + "/doctorid.txt", "1 = Dr. Ali\n2 = Dr. Sara\n3 = Dr. Ahmed");
			EnsureFileExists(DOCTORS_DIR + "/doctorfees.txt", "1 = 2000\n2 = 2500\n3 = 1500");
			EnsureFileExists(DOCTORS_DIR + "/1_monday.txt", "ID:1 - Dr. Ali (Cardiologist) - 10am to 2pm");
			EnsureFileExists(DOCTORS_DIR + "/2_tuesday.txt", "ID:2 - Dr. Sara (Neurologist) - 11am to 3pm");
			EnsureFileExists(DOCTORS_DIR + "/3_wednesday.txt", "ID:3 - Dr. Ahmed (Pediatrician) - 9am to 1pm");
			EnsureFileExists(DOCTORS_DIR + "/4_thursday.txt", "ID:1 - Dr. Ali (Cardiologist) - 2pm to 6pm");
			EnsureFileExists(DOCTORS_DIR + "/5_friday.txt", "ID:2 - Dr. Sara (Neurologist) - 12pm to 4pm");

			Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
			ShowWindow(ThisConsole, MAXIMIZE);
			Console.ForegroundColor = ConsoleColor.Red;
			Title();
			Choice();
		}

		static void EnsureFileExists(string path, string defaultContent)
		{
			if (!File.Exists(path))
			{
				File.WriteAllText(path, defaultContent);
			}
		}

		static void Title()
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
			Console.WriteLine(@"#     #                                               #     #                                                                   #####                                  ");
			Console.WriteLine(@"#     #  ####   ####  #####  # #####   ##   #         ##   ##   ##   #    #   ##    ####  ###### #    # ###### #    # #####    #     # #   #  ####  ##### ###### #    #");
			Console.WriteLine(@"#     # #    # #      #    # #   #    #  #  #         # # # #  #  #  ##   #  #  #  #    # #      ##  ## #      ##   #   #      #        # #  #        #   #      ##  ##");
			Console.WriteLine(@"####### #    #  ####  #    # #   #   #    # #         #  #  # #    # # #  # #    # #      #####  # ## # #####  # #  #   #       #####    #    ####    #   #####  # ## #");
			Console.WriteLine(@"#     # #    #      # #####  #   #   ###### #         #     # ###### #  # # ###### #  ### #      #    # #      #  # #   #            #   #        #   #   #      #    #");
			Console.WriteLine(@"#     # #    # #    # #      #   #   #    # #         #     # #    # #   ## #    # #    # #      #    # #      #   ##   #      #     #   #   #    #   #   #      #    #");
			Console.WriteLine(@"#     #  ####   ####  #      #   #   #    # ######    #     # #    # #    # #    #  ####  ###### #    # ###### #    #   #       #####    #    ####    #   ###### #    #");
			Console.WriteLine();
			Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
			Console.WriteLine();
		}

		static void Choice()
		{
			while (true)
			{
				Console.ForegroundColor = ConsoleColor.White;
				Console.WriteLine();
				Console.Write("\t\t\t\t\t\t\t\t::Enter Your Choice::");
				Console.WriteLine("\n");
				Console.WriteLine("\t\t\t\t\t\t\t\tFor Doctor's Appointment:         (1)");
				Console.WriteLine("\t\t\t\t\t\t\t\tTo Search for Appointment         (2)");
				Console.WriteLine("\t\t\t\t\t\t\t\tTo see All Appointments List:     (3)");
				Console.WriteLine("\t\t\t\t\t\t\t\tBlood Bank:                       (4)");
				Console.WriteLine("\t\t\t\t\t\t\t\tTo Exit:                          (5)");
				Console.Write("\n\t\t\t\t\t\t\t\tEnter Your Choice:");

				if (!int.TryParse(Console.ReadLine(), out int Choice))
				{
					Console.Clear();
					Title();
					Console.WriteLine("\n\t\t\t\t\t\tInvalid input! Please enter a number.\n");
					continue;
				}

				switch (Choice)
				{
					case 1:
						Console.Clear();
						Appointment();
						break;
					case 2:
						Console.Clear();
						SearchAppointment();
						break;
					case 3:
						Console.Clear();
						AppointmentList();
						break;
					case 4:
						Console.Clear();
						BloodBank();
						break;
					case 5:
						Console.Clear();
						Exit();
						return;
					default:
						Console.Clear();
						Title();
						Console.WriteLine("\n\t\t\t\t\t\tInvalid choice! Try again.\n");
						break;
				}
			}
		}

		static void SearchAppointment()
		{
			Title();
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine("Enter Patient's CNIC: ");
			string pat_cnic = Console.ReadLine().Trim();
			if (string.IsNullOrEmpty(pat_cnic))
			{
				Console.WriteLine("\nCNIC cannot be empty!");
				ReturnToMenu();
				return;
			}

			string filePath = APPOINTMENTS_DIR + "/" + pat_cnic + ".txt";
			Console.WriteLine("\n\n------------------------------------------------");
			Console.WriteLine(pat_cnic + "'s Appointment List");
			Console.WriteLine("------------------------------------------------");

			if (File.Exists(filePath))
			{
				string content = File.ReadAllText(filePath);
				Console.WriteLine(content);
			}
			else
			{
				Console.WriteLine("\n\nYou do not have any appointment");
			}
			ReturnToMenu();
		}

		static void Appointment()
		{
			Console.Clear();
			Title();
			Console.ForegroundColor = ConsoleColor.White;

			if (!File.Exists(DOCTORS_LIST_FILE))
			{
				Console.WriteLine("Doctors list file missing!");
				ReturnToMenu();
				return;
			}
			string doctors = File.ReadAllText(DOCTORS_LIST_FILE);
			Console.WriteLine(doctors);

			Dictionary<string, string> doc_id = new Dictionary<string, string>();
			Dictionary<string, string> doc_fees = new Dictionary<string, string>();

			try
			{
				doc_id = File.ReadAllLines(DOCTORS_DIR + "/doctorid.txt")
					.Select(l => l.Split(new[] { '=' }))
					.ToDictionary(s => s[0].Trim(), s => s[1].Trim());
				doc_fees = File.ReadAllLines(DOCTORS_DIR + "/doctorfees.txt")
					.Select(l => l.Split(new[] { '=' }))
					.ToDictionary(s => s[0].Trim(), s => s[1].Trim());
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error reading doctor files: {ex.Message}");
				ReturnToMenu();
				return;
			}

			// Day selection with loop (no goto)
			string day = "";
			bool validDay = false;
			while (!validDay)
			{
				Console.WriteLine("\n\nSelect Appointment Day (Monday to Friday):");
				day = Console.ReadLine().Trim().ToLower();
				string dayFile = "";
				switch (day)
				{
					case "monday": dayFile = "1_monday.txt"; validDay = true; break;
					case "tuesday": dayFile = "2_tuesday.txt"; validDay = true; break;
					case "wednesday": dayFile = "3_wednesday.txt"; validDay = true; break;
					case "thursday": dayFile = "4_thursday.txt"; validDay = true; break;
					case "friday": dayFile = "5_friday.txt"; validDay = true; break;
					case "saturday":
					case "sunday":
						Console.WriteLine(" Saturday and Sunday are OFF. Please select a weekday.");
						break;
					default:
						Console.WriteLine("Invalid day entered. Try again.");
						break;
				}
				if (validDay)
				{
					Console.Clear();
					Title();
					string schedulePath = DOCTORS_DIR + "/" + dayFile;
					if (File.Exists(schedulePath))
					{
						string schedule = File.ReadAllText(schedulePath);
						Console.WriteLine("\n" + schedule);
					}
					else
					{
						Console.WriteLine("\nSchedule not available for this day.");
						validDay = false;
					}
				}
			}

			Console.WriteLine("\nEnter Doctor's ID for Appointment:");
			string sel_doc = Console.ReadLine().Trim();
			if (!doc_id.ContainsKey(sel_doc))
			{
				Console.WriteLine("Invalid Doctor ID.");
				ReturnToMenu();
				return;
			}
			string doc_name = doc_id[sel_doc];

			Console.Clear();
			Console.WriteLine("\nAssign Appointment Number:");
			string appointment_num = Console.ReadLine().Trim();
			Console.WriteLine("\nEnter Patient's Name:");
			string pat_name = Console.ReadLine().Trim();
			Console.WriteLine("\nEnter Patient's Age:");
			string pat_age = Console.ReadLine().Trim();
			Console.WriteLine("\nEnter Patient's Contact Number:");
			string pat_contact = Console.ReadLine().Trim();
			Console.WriteLine("\nEnter Patient's CNIC:");
			string pat_cnic = Console.ReadLine().Trim();

			if (string.IsNullOrEmpty(pat_name) || string.IsNullOrEmpty(pat_cnic))
			{
				Console.WriteLine("Name and CNIC are required.");
				ReturnToMenu();
				return;
			}

			string fees = doc_fees.ContainsKey(sel_doc) ? doc_fees[sel_doc] : "Unknown";
			DateTime now = DateTime.Now;
			string appointmentContent = Environment.NewLine + "Appointment Receipt:\n" + "--------------------" +
				"\nAppointment Day:  " + day +
				"\nAppointment For:  " + doc_name +
				"\nAppointment Number:  " + appointment_num +
				"\nPatient's Name:  " + pat_name +
				"\nPatient's Age:  " + pat_age +
				"\nPatient's Phone number:  " + pat_contact +
				"\nPatient's CNIC:  " + pat_cnic +
				"\nYour charges:  " + fees +
				"\nDate & Time:  " + now + "\n::::\n\n";

			string patientFile = APPOINTMENTS_DIR + "/" + pat_cnic + ".txt";
			File.WriteAllText(patientFile, appointmentContent);
			File.AppendAllText(ALL_APPOINTMENTS_FILE, appointmentContent);

			Console.Clear();
			Console.WriteLine("Your Appointment has been created\n\n");
			Console.WriteLine(appointmentContent);
			ReturnToMenu();
		}

		static void AppointmentList()
		{
			Title();
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine("\t\t\t\t__________________ ");
			Console.WriteLine("\t\t\t\t--------------------------------------------------");
			Console.WriteLine("\t\t\t\t||\t\t Appointment List:\t\t||");
			Console.WriteLine("\t\t\t\t__________________ ");
			Console.WriteLine("\t\t\t\t-------------------------------------------------- ");

			if (File.Exists(ALL_APPOINTMENTS_FILE))
			{
				string content = File.ReadAllText(ALL_APPOINTMENTS_FILE);
				Console.WriteLine(content);
			}
			else
			{
				Console.WriteLine("No appointments exist.");
			}
			ReturnToMenu();
		}

		static void BloodBank()
		{
			while (true)
			{
				Title();
				Console.ForegroundColor = ConsoleColor.White;
				Console.WriteLine();
				Console.Write("\t\t\t\t\t::Enter Your Choice::");
				Console.WriteLine("\n");
				Console.WriteLine("\t\t\t\t\t\t\tTo donate blood:      (1)");
				Console.WriteLine("\t\t\t\t\t\t\tTo request blood:     (2)");
				Console.WriteLine("\t\t\t\t\t\t\tTo see Donors List:   (3)");
				Console.WriteLine("\t\t\t\t\t\t\tTo see Requests Lists:(4)");
				Console.WriteLine("\t\t\t\t\t\t\tTo Exit:              (5)");
				Console.Write("\t\t\t\t\t:");

				if (!int.TryParse(Console.ReadLine(), out int Choice))
				{
					Console.Clear();
					Console.WriteLine("Invalid input!");
					continue;
				}

				switch (Choice)
				{
					case 1:
						Console.Clear();
						Donate();
						break;
					case 2:
						Console.Clear();
						ToRequestBlood();
						break;
					case 3:
						Console.Clear();
						List();
						break;
					case 4:
						Console.Clear();
						ReqList();
						break;
					case 5:
						Console.Clear();
						Exit();
						return;
					default:
						Console.Clear();
						Console.WriteLine("Invalid choice!");
						break;
				}
			}
		}

		static void Donate()
		{
			Title();
			Console.ForegroundColor = ConsoleColor.White;

			Console.WriteLine("\t\t\t\t\tEnter Name:");
			Console.Write("\t\t\t\t\t");
			string name = Console.ReadLine().Trim();
			if (string.IsNullOrEmpty(name))
			{
				Console.WriteLine("Name is required.");
				ReturnToMenu();
				return;
			}

			Console.WriteLine("\n\t\t\t\t\tEnter your Medical ID:");
			Console.Write("\t\t\t\t\t");
			string id = Console.ReadLine().Trim();

			Console.WriteLine("\n\t\t\t\t\tEnter your Blood group:");
			Console.Write("\t\t\t\t\t");
			string blood = Console.ReadLine().Trim().ToUpper();

			Console.WriteLine("\n\t\t\t\t\tNumber of Bottles Donate:");
			Console.Write("\t\t\t\t\t");
			if (!int.TryParse(Console.ReadLine(), out int no))
			{
				Console.WriteLine("Invalid number.");
				ReturnToMenu();
				return;
			}

			Console.WriteLine("\n\t\t\t\t\tEnter your age:");
			Console.Write("\t\t\t\t\t");
			if (!int.TryParse(Console.ReadLine(), out int age))
			{
				Console.WriteLine("Invalid age.");
				ReturnToMenu();
				return;
			}

			if (age <= 18)
			{
				Console.WriteLine("\n\t\t\t\t\tSorry! You are Under-Age. Cannot donate.");
				ReturnToMenu();
				return;
			}

			Console.WriteLine("\n\t\t\t\t\tEnter phone number of Donor:");
			Console.Write("\t\t\t\t\t");
			string phone = Console.ReadLine().Trim();

			Console.WriteLine("\n\t\t\t\t\tEnter E-mail of Donor:");
			Console.Write("\t\t\t\t\t");
			string email = Console.ReadLine().Trim();

			Console.WriteLine("\n\t\t\t\t\tYour data has been successfully added to our system. Thanks!");
			Console.Write("\t\t\t\tDo you want Receipt (yes/no): ");
			string opt = Console.ReadLine().Trim().ToLower();
			if (opt == "y" || opt == "yes")
			{
				DateTime now = DateTime.Now;
				string receiptContent = $"\n\t\t\t\t\tBlood Donor Receipt:" +
					$"\n\t\t\t\t\tName: {name}" +
					$"\n\t\t\t\t\tMedical ID: {id}" +
					$"\n\t\t\t\t\tBlood Group: {blood}" +
					$"\n\t\t\t\t\tNo of Blood: {no}" +
					$"\n\t\t\t\t\tYour Age: {age}" +
					$"\n\t\t\t\t\tDonor Phone: {phone}" +
					$"\n\t\t\t\t\tDonor Email: {email}" +
					$"\n\t\t\t\t\tDate & Time: {now}\n\t\t\t\t\t::::\n";
				File.AppendAllText(DONORS_RECEIPT_FILE, receiptContent);
				Console.Clear();
				Title();
				Console.WriteLine(receiptContent);
			}
			else
			{
				Console.WriteLine("\t\t\t\t\tData saved successfully.");
			}
			ReturnToMenu();
		}

		static void List()
		{
			Title();
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine("\t\t\t\t__________________ ");
			Console.WriteLine("\t\t\t\t--------------------------------------------------");
			Console.WriteLine("\t\t\t\t||\t\t Blood Donor List:\t\t||");
			Console.WriteLine("\t\t\t\t__________________ ");
			Console.WriteLine("\t\t\t\t-------------------------------------------------- ");

			if (File.Exists(DONORS_RECEIPT_FILE))
			{
				string content = File.ReadAllText(DONORS_RECEIPT_FILE);
				Console.WriteLine(content);
			}
			else
			{
				Console.WriteLine("No donors yet.");
			}
			ReturnToMenu();
		}

		static void ToRequestBlood()
		{
			Title();
			Console.ForegroundColor = ConsoleColor.White;
			Category();

			string[] bloodGroups = { "A+", "B+", "O+", "AB+", "A-", "B-", "O-", "AB-" };
			string[] availability = { "Available", "Available", "Available", "Not Available", "Available", "Available", "Not Available", "Available" };
			for (int i = 0; i < bloodGroups.Length; i++)
			{
				Console.WriteLine($"\t\t\t\t Blood Group {bloodGroups[i]} ({availability[i]})");
				Console.WriteLine("\t\t\t\t -------------------------------------------------");
			}

			Console.Write("\t\t\t\t Do you want to proceed? (yes/no): ");
			string proceed = Console.ReadLine().Trim().ToLower();
			if (!(proceed == "y" || proceed == "yes"))
			{
				Exit();
				return;
			}

			Console.Clear();
			Title();
			Category();

			Console.WriteLine("\t\t\t\t\tEnter Name:");
			Console.Write("\t\t\t\t\t");
			string name = Console.ReadLine().Trim();
			if (string.IsNullOrEmpty(name))
			{
				Console.WriteLine("Name required.");
				ReturnToMenu();
				return;
			}

			Console.WriteLine("\n\t\t\t\t\tEnter your Medical ID:");
			Console.Write("\t\t\t\t\t");
			string id = Console.ReadLine().Trim();

			Console.WriteLine("\n\t\t\t\t\tEnter Blood group you Required:");
			Console.Write("\t\t\t\t\t");
			string blood = Console.ReadLine().Trim().ToUpper();

			// Check availability
			bool available = true;
			if (blood == "O-" || blood == "AB+")
			{
				available = false;
			}
			if (!available)
			{
				Console.WriteLine("\t\t\t\tSorry! This Blood Group is not available at this time.");
				ReturnToMenu();
				return;
			}
			if (!bloodGroups.Contains(blood))
			{
				Console.WriteLine("\n\t\t\t\t\tThere is no category available for this group.");
				ReturnToMenu();
				return;
			}

			Console.WriteLine("\n\t\t\t\t\tNumber of Bottles you Required:");
			Console.Write("\t\t\t\t\t");
			if (!int.TryParse(Console.ReadLine(), out int no) || no <= 0)
			{
				Console.WriteLine("Invalid number.");
				ReturnToMenu();
				return;
			}

			Console.WriteLine("\n\t\t\t\t\tEnter your age:");
			Console.Write("\t\t\t\t\t");
			if (!int.TryParse(Console.ReadLine(), out int age) || age < 0)
			{
				Console.WriteLine("Invalid age.");
				ReturnToMenu();
				return;
			}

			Console.WriteLine("\n\t\t\t\t\tEnter phone number of Acceptor:");
			Console.Write("\t\t\t\t\t");
			string phone = Console.ReadLine().Trim();

			Console.WriteLine("\n\t\t\t\t\tEnter e-mail of Acceptor:");
			Console.Write("\t\t\t\t\t");
			string email = Console.ReadLine().Trim();

			int payment = 4000 * no;
			Console.WriteLine($"\n\t\t\t\tYour Charges: {payment}");

			Console.WriteLine("\n\t\t\t\t\tYour data has been successfully added to our system. Thanks!");
			Console.Write("\t\t\t\tDo you want Receipt (yes/no): ");
			string opt = Console.ReadLine().Trim().ToLower();
			if (opt == "y" || opt == "yes")
			{
				DateTime now = DateTime.Now;
				string receiptContent = $"\n\t\t\t\t\tBlood Acceptor Receipt:" +
					$"\n\t\t\t\t\tName: {name}" +
					$"\n\t\t\t\t\tMedical ID: {id}" +
					$"\n\t\t\t\t\tBlood Group: {blood}" +
					$"\n\t\t\t\t\tNo of Blood: {no}" +
					$"\n\t\t\t\t\tAcceptor Phone: {phone}" +
					$"\n\t\t\t\t\tAcceptor Email: {email}" +
					$"\n\t\t\t\t\tYour charges: {payment}" +
					$"\n\t\t\t\t\tDate & Time: {now}\n\t\t\t\t\t::::\n";
				File.AppendAllText(REQUESTS_FILE, receiptContent);
				Console.Clear();
				Title();
				Console.WriteLine(receiptContent);
			}
			ReturnToMenu();
		}

		static void Category()
		{
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine("\t\t\t\t__________________ ");
			Console.WriteLine("\t\t\t\t--------------------------------------------------");
			Console.WriteLine("\t\t\t\t||\tEnter which category Blood you want:\t||");
			Console.WriteLine("\t\t\t\t__________________ ");
			Console.WriteLine("\t\t\t\t-------------------------------------------------- ");
			Console.WriteLine();
		}

		static void ReqList()
		{
			Title();
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine("\t\t\t\t__________________ ");
			Console.WriteLine("\t\t\t\t--------------------------------------------------");
			Console.WriteLine("\t\t\t\t||\t\t Blood Request List:\t\t||");
			Console.WriteLine("\t\t\t\t__________________ ");
			Console.WriteLine("\t\t\t\t-------------------------------------------------- ");

			if (File.Exists(REQUESTS_FILE))
			{
				string content = File.ReadAllText(REQUESTS_FILE);
				Console.WriteLine(content);
			}
			else
			{
				Console.WriteLine("No requests yet.");
			}
			ReturnToMenu();
		}

		static void ReturnToMenu()
		{
			Console.WriteLine("\n\nDo you want to go back to Main Menu? (yes/no): ");
			string opt = Console.ReadLine().Trim().ToLower();
			if (opt == "yes" || opt == "y")
			{
				Console.Clear();
				Title();
				Choice();
			}
			else
			{
				Exit();
			}
		}

		static void Exit()
		{
			Title();
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine("\n\t\t\t\tThanks for Choosing our Hospital Management System:\n\t\t\t\tWe are waiting for you to come here again!");
			Console.ReadLine();
			Environment.Exit(0);
		}
	}
}