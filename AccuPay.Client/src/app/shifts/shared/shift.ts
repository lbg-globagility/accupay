export interface Shift {
    id: number;
    employeeId: number;
    employeeNumber: string;
    employeeName: string;
    employeeType: string;
    dateSched: Date;
    startTime: Date;
    endTime: Date;
    breakStartTime: Date;
    breakLength: number;
    isRestDay: boolean;  
  }
  