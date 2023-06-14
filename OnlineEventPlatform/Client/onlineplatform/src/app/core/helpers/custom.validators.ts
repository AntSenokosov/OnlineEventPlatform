import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export class CustomValidators {

  static patternValidator(regex: RegExp, error: ValidationErrors): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) {
        // If the control is empty, return no error
        return null;
      }

      // Test the value of the control against the supplied regex
      const valid = regex.test(control.value);

      // Return the error object if the value does not match the pattern, or null if it does
      return valid ? null : error;
    };
  }

  static passwordMatchValidator(control: AbstractControl): void {
    const password = control.get('password')?.value; // Get the password value from the 'password' form control
    const confirmPassword = control.get('confirmPassword')?.value; // Get the confirmPassword value from the 'confirmPassword' form control

    // Compare if the password and confirmPassword match
    if (password !== confirmPassword) {
      // If they don't match, set the 'NoPasswordMatch' error on the 'confirmPassword' form control
      control.get('confirmPassword')?.setErrors({ NoPasswordMatch: true });
    } else {
      // If they match, clear the 'NoPasswordMatch' error on the 'confirmPassword' form control
      control.get('confirmPassword')?.setErrors(null);
    }
  }

}
