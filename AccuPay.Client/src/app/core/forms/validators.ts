import {
  AbstractControl,
  FormGroup,
  ValidationErrors,
  ValidatorFn
} from '@angular/forms';
import { isNumber } from 'lodash';
import { sharedStylesheetJitUrl } from '@angular/compiler';
import { OrganizationAcknowledgementThresholdsComponent } from 'src/app/main/organization/organization-acknowledgement-thresholds/organization-acknowledgement-thresholds.component';

export class CustomValidators {
  public static equalTo(comparedField: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const comparedControl = control.root.get(comparedField);
      const comparedValue = comparedControl ? comparedControl.value : null;

      if (control.value === comparedValue) {
        return null;
      }

      return { equalTo: true };
    };
  }

  static moreThan(first: string, second: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (control.parent == null) {
        return null;
      }

      if (control.value == null) {
        return null;
      }

      const firstnumber = control.parent.get('first').value;

      if (control.value > firstnumber) {
        return null;
      }
      return { moreThan: true };
    };
  }

  public static notEmpty(key: string): ValidatorFn {
    return (group: FormGroup): ValidationErrors | null => {
      const subGroup = <FormGroup>group.controls[key];

      const hasAtleastOne =
        subGroup &&
        subGroup.controls &&
        Object.keys(subGroup.controls).length > 0;

      return hasAtleastOne ? null : { notEmpty: true };
    };
  }

  static isPasswordAcceptable(
    control: AbstractControl
  ): ValidationErrors | null {
    const value = control.value as string;

    if (!value) {
      return null;
    }

    // Regex for checking if string contains at least one of each:
    // • Uppercase letter
    // • Lowercase letter
    // • Numerical digit
    const regex = /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$/g;
    const isValid = regex.test(value);

    if (isValid) {
      return null;
    }

    return { isPasswordAcceptable: true };
  }

  static DecimalValidator(control: AbstractControl) {
    const { value } = control;

    if (String(value).trim().length > 0) {
      if (!CustomValidators.isNumeric(value)) {
        return { validateInteger: true };
      }
    }
    return null;
  }

  static isNumeric(x) {
    return (
      !isNaN(x) &&
      typeof x !== 'object' &&
      x != Number.POSITIVE_INFINITY &&
      x != Number.NEGATIVE_INFINITY
    );
  }
}
