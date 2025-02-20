# Study Center Management System
## 1. Introduction

The Study Center Management System is designed to manage students, teachers, classes, subjects, exams, and results efficiently. This documentation provides a comprehensive overview of the workflow, data flow, and interactions between different system components to ensure smooth operation and administration.

---

## 2. Key Features

- **User Roles & Permissions:** Role-based access control for administrators, teachers, and students.
- **User Management:** Secure registration, authentication, and activity logging.
- **Student & Teacher Management:** Enrollment, assignment to classes and subjects, and status tracking.
- **Class & Subject Management:** Creation, scheduling, and teacher-subject assignments.
- **Exam & Assessment Management:** Exam creation, student submissions, automated evaluation, and reporting.
- **Logging & System Monitoring:** Tracks user actions, deleted records, and system activity.

---

## 3. System Modules & Workflows

### **User Management**
- **User Registration:** New users (students, teachers, administrators) are registered.
- **Authentication:** Users log in using their credentials.
- **Permission Assignment:** Different access levels are assigned based on roles.
- **User Activity Logging:** Tracks actions performed by users.

### **Student Management**
- **Student Registration:** A new student is added to the system.
- **Assign Grade Level:** Students are assigned to a specific grade.
- **Assign Classes:** Students are placed into relevant classes.
- **Track Status:** Active, graduated, or transferred.
- **Deletion Logging:** If a student is removed, logs are created for reference.

### **Teacher Management**
- **Teacher Registration:** New teachers are added to the system.
- **Assign Education Level:** Teachers are categorized by qualification.
- **Subject Assignment:** Teachers are assigned to specific subjects.
- **Track Status:** Active or inactive.
- **Deletion Logging:** If a teacher is removed, logs are created for reference.

### **Class & Subject Management**
- **Create Classes:** Define class name, capacity, and description.
- **Assign Teachers to Subjects:** Link teachers to subjects and grade levels.
- **Schedule Meetings:** Define meeting times for each class.
- **Manage Subjects:** Define subjects, grade levels, fees, and mandatory status.

### **Exam & Assessment Management**
- **Create Exams:** Define exam name, subject, date, and total marks.
- **Add Questions:** Assign questions and their corresponding marks.
- **Record Answers:** Students submit their answers.
- **Evaluate Results:** Calculate marks obtained and update results.
- **Generate Reports:** Provide performance insights per student.

### **Logging & System Monitoring**
- **Action Logging:** Track all database changes (insertion, updates, deletion).
- **Deleted Student & Teacher Logs:** Maintain history of removed records.
- **User Activity Logs:** Track system usage per user.

---

## **Technologies Used**
- **API Architecture:** RESTful API  
- **Framework:** .NET Core (C#)  
- **Database:** Microsoft SQL Server & T-SQL with ADO.NET  
- **Architecture:** 3-tier architecture  
