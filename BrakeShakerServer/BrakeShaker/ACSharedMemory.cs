using System;
using System.Runtime.InteropServices;

namespace BrakeShaker
{
    public enum AC_STATUS
    {
        AC_OFF = 0,
        AC_REPAY = 1,
        AC_LIVE = 2,
        AC_PAUSE = 3
    }

    public enum AC_SESSION_TYPE
    {
        AC_UNKNOWN = -1,
        AC_PRACTICE = 0,
        AC_QUALIFY = 1,
        AC_RACE = 2,
        AC_HOTLAP = 3,
        AC_TIME_ATTACK = 4,
        AC_DRIFT = 5,
        AC_DRAG = 6
    }

    public enum AC_FLAG_TYPE
    {
        AC_NO_FLAG = 0,
        AC_BLUE_FLAG = 1,
        AC_YELLOW_FLAG = 2,
        AC_BLACK_FLAG = 3,
        AC_WHITE_FLAG = 4,
        AC_CHECKERED_FLAG = 5,
        AC_PENALTY_FLAG = 6
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Unicode)]
    public struct ACStatic
    {
        //  ---------------SPageFileStatic---------------
        // The following members are initialized when the instance starts and never changes until the instance is closed.
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public string smVersion;    //Version of the SharedMemory structure
       [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public string acVersion;    //Version of Assetto Corsa
        public int numberOfSessions;    //Number of sessions in this instance -0
        public int numCars; //Max number of possible cars on track -0
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string carModel; //Name of the player’s car
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string track;    //Name of the track
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string playerName;   //Name of the player
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string playerSurname;    //Surname of the player
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string playerNick;   //Nickname of the player
        public int sectorCount; //Number of track sectors -0
        public float maxTorque; //Max torque value of the player’s car -0
        public float maxPower;  //Max power value of the player’s car -0
        public int maxRpm;  //Max rpm value of the player’s car -0
        public float maxFuel;   //Max fuel value of the player’s car -0
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] suspensionMaxTravel; //Max travel distance of each tire [FrontLeft, FrontRight, RearLeft, RearRight]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] tyreRadius;  //Radius of each tire [FrontLeft, FrontRight, RearLeft, RearRight]
        public float maxTurboBoost; //Max turbo boost value of the player’s car -0
        public int penaltiesEnabled;    //Cut penalties enabled: 1 (true) or 0 (false) -0
        public float aidFuelRate;   //Fuel consumption rate: 0 (no cons), 1 (normal), 2 (double cons) etc. -0
        public float aidTyreRate;   //Tire wear rate: 0 (no wear), 1 (normal), 2 (double wear) etc. -0
        public float aidMechanicalDamage;   //Damage rate: 0 (no damage) to 1 (normal) -0
        public int aidAllowTyreBlankets;    //Player starts with hot (optimal temp) tires: 1 (true) or 0 (false) -0
        public float aidStability;  //Stability aid: 0 (no aid) to 1 (full aid) -0
        public int aidAutoClutch;   //If player’s car has the “auto clutch” feature enabled : 0 or 1 -0
        public int aidAutoBlip; //If player’s car has the “auto blip” feature enabled : 0 or 1 -0
        public int hasDRS;  //If player’s car has the “DRS” system: 0 or 1 -0
        public int hasERS;  //If player’s car has the “ERS” system: 0 or 1 -0
        public int hasKERS; //If player’s car has the “KERS” system: 0 or 1 -0
        public float kersMaxJ;  //Max KERS/ERS Joule value of the player’s car -0
        public int engineBrakeSettingsCount;    //Count of possible engine brake settings of the player’s car -0
        public int ersPowerControllerCount; //Count of the possible power controllers of the player’s car -0
        public float trackSPlineLength; //Length of the spline of the selected track -0
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string trackConfiguration;   //Name of the track’s layout (only multi-layout tracks)
    }
    public struct ACPhysic
    {
        //  ---------------SPageFilePhysics---------------
        // The following members change at each graphic step. They all refer to the player’s car.
        public int packetId;    //Index of the shared memory’s current step -0
        public float gas;   //Value of gas pedal: 0 to 1 (fully pressed) -0
        public float brake; //Value of brake pedal: 0 to 1 (fully pressed) -0
        public float fuel;  //Liters of fuel in the car -0
        public int gear;    //Selected gear (0 is reverse, 1 is neutral, 2 is first gear ) -0
        public int rpms;    //Value of rpm -0
        public float steerAngle;    //Angle of steer -0
        public float speedKmh;  //Speed in Km/h -0
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public float[] velocity;    //Velocity for each axis [x, y, z]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public float[] accG;    //G­-force for each axis [x, y, z]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] wheelSlip;   //Spin speed of each tire [FrontLeft, FrontRight, RearLeft, RearRight]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] wheelLoad;   //Load on each tire [FrontLeft, FrontRight, RearLeft, RearRight]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] wheelsPressure;  //Pressure of each tire [FrontLeft, FrontRight, RearLeft, RearRight]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] wheelAngularSpeed;   //Angular speed of each tire [FrontLeft, FrontRight, RearLeft, RearRight]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] tyreWear;    //Current wear of each tire [FrontLeft, FrontRight, RearLeft, RearRight]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] tyreDirtyLevel;  //Dirt level on each tire [FrontLeft, FrontRight, RearLeft, RearRight]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] tyreCoreTemperature; //Core temperature of each tire [FrontLeft, FrontRight, RearLeft, RearRight]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] camberRad;   //Camber of each tire in Radian [FrontLeft, FrontRight, RearLeft, RearRight]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] suspensionTravel;    //Suspension travel for each tire [FrontLeft, FrontRight, RearLeft, RearRight]
        public float drs;   //If DRS is present and enabled: 0 (false) or 1 (true) -0
        public float tc;    //Slip ratio limit for the traction control (if enabled) -0
        public float heading;   //Heading of the car on world coordinates -0
        public float pitch; //Pitch of the car on world coordinates -0
        public float roll;  //Roll of the car on world coordinates -0
        public float cgHeight;  //Height of Center of Gravity -0
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public float[] carDamage;   //Level of damage for each car section (only first 4 are valid)
        public int numberOfTyresOut;    //How many tires are allowed to stay out of the track to not receive a penalty -0
        public int pitLimiterOn;    //If pit limiter is enabled: 0 (false) or 1 (true) -0
        public float abs;   //Slip ratio limit for the ABS (if enabled) -0
        public float kersCharge;    //KERS/ERS battery charge: 0 to 1 -0
        public float kersInput; //KERS/ERS input to engine: 0 to 1 -0
        public int autoShifterOn;   //If auto shifter is enabled: 0 (false) or 1 (true) -0
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public float[] rideHeight;    //Right heights: front and rear
        public float turboBoost;    //Turbo boost-0
        public float ballast;   //Kilograms of ballast added to the car (only in multi-player) -0
        public float airDensity;    //Air density -0
        public float airTemp;   //Ambient temperature -0
        public float roadTemp;  //Road temperature -0
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public float[] localAngularVel; //Angular velocity of the car [x, y, z]
        public float finalFF;   //Current Force Feedback value -0
        public float performanceMeter;  //Performance meter compared to the best lap -0
        public int engineBrake; //Engine brake setting -0
        public int ersRecoveryLevel;    //ERS recovery level -0
        public int ersPowerLevel;   //ERS selected power controller -0
        public int ersHeatCharging; //ERS changing: 0 (Motor) or 1 (Battery) -0
        public int ersIsCharging;   //If ERS battery is recharging: 0 (false) or 1 (true) -0
        public float kersCurrentKJ; //KERS/ERS KiloJoule spent during the lap -0
        public int drsAvailable;    //If DRS is available (DRS zone): 0 (false) or 1 (true) -0
        public int drsEnabled;  //If DRS is enabled: 0 (false) or 1 (true) -0
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] brakeTemp;   //Brake temp for each tire [FrontLeft, FrontRight, RearLeft, RearRight]
    }
    public struct ACGraphic
    {
        //  ---------------SPageFileGraphic---------------
        //The following members change at each graphical step. They all refer to the player’s car.
        public int packetId;    //Index of the shared memory’s current step -0
        public AC_STATUS status;    //Status of the instance - AC_OFF
        public AC_SESSION_TYPE session; //Session type - AC_PRACTICE
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public string currentTime;  //Current lap time
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public string lastTime; //Last lap time
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public string bestTime; //Best lap time
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public string split;    //Time in sector
        public int completedLaps;   //Number of completed laps by the player -0
        public int position;    //Current player position (standings) -0
        public int iCurrentTime;    //Current lap time -0
        public int iLastTime;   //Last lap time -0
        public int iBestTime; //Best lap time -0
        public float sessionTimeLeft;   //Time left until session is closed -0
        public float distanceTraveled; //Distance traveled during the instance -0
        public int isInPit; //If player’s car is stopped in the pit: 0 (false) or 1 (true) -0
        public int currentSectorIndex; //Current sector index -0
        public int lastSectorTime;  //Last sector time -0
        public int numberOfLaps;    //Number of laps needed to close the session -0
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string tyreCompound; //Current tire compound
        public float replayTimeMultiplier;  //Replay multiplier -0
        public float normalizedCarPosition; //Car position on the track’s spline -0
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public float[] carCoordinates;  //Car position on world coordinates [x, y, z]
        public float penaltyTime;   //Time of penalty -0
        public AC_FLAG_TYPE flag;   //Type of flag being shown - AC_NO_FLAG
        public int idealLineOn; //If ideal line is enabled: 0 (false) or 1 (true) -0
        public int isInPitLane; //If player’s car is in the pit lane: 0 (false) or 1 (true) -0
        public float surfaceGrip;   //Current grip of the track’s surface -0
    }

}
