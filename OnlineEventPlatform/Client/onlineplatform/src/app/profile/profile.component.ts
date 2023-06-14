import { Component, OnInit } from '@angular/core';
import { ProfileService } from './profile.service';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { Router } from '@angular/router';
import {AuthService} from '../auth/auth.service';
import { User } from '../auth/user.model';
import { CustomValidators } from '../core/helpers/custom.validators';
import { GenearateTwoFactory } from './dtos/generatetwofactory.request';
import { DisableTwoFactoryRequest } from './dtos/disabletwofactory.request';
import { VerifyTwoFactoryRequest } from './dtos/verifytwofactory.request';
import { LoginRequest } from '../auth/dtos/loginrequest';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit{
  user! : User;
  settingsForm!: FormGroup;
  twoFAForm!: FormGroup;
  verifyTwoFAForm!: FormGroup;
  twoFADisableForm!: FormGroup;

  errors: object = {};
  isSubmitting = false;
  passDialogOpened = false;
  twoFAEnableDialogOpened = false;
  twoFADisableDialogOpened = false;
  resetPasswordForm!: FormGroup;
  showInvalidDialog = false;

  twoFAQRCreated = false;
  twoFAVerified = false;
  twoFADisabledSucess = false;
  twoFAVerificationError = false;
  twoFAQRImageURL!: SafeUrl;
  manualEntrySetupCode = '';
  currentDialogPage = 1;
  twoFAButtonText = '';
  twoFAStatus = '';

  constructor(
    private profileService : ProfileService,
    private authService : AuthService,
    private router: Router,
    private fb: FormBuilder,
    private sanitizer: DomSanitizer
  )
  {
    this.settingsForm = this.fb.group({
      firstName: '',
      lastName: '',
      email : ''
    });
  }

  ngOnInit(): void {
    this.profileService.getProfile().subscribe((user) => {
      if (user) {
        this.user = {} as User;
        Object.assign(this.user, user);
        this.settingsForm.patchValue(this.user);
        this.twoFATextLogic();
      } else {
        console.error('Current user is not available');
      }
    });
  }

  logout()
  {
    this.authService.logout();
    this.router.navigateByUrl('/');
  }

  submitForm() {
    this.isSubmitting = true;

    // update the model
    this.updateUser(this.settingsForm.value);

    this.profileService.updateProfile(this.user).subscribe(
      (updatedUser) => {
        this.pageRefresh();
      }, //this.router.navigateByUrl('/profile/' + updatedUser.username),
      (err) => {
        this.errors = err;
        this.isSubmitting = false;
      }
    );
  }

  changePasswordHandler(){

  }





  createPasswordFormGroup(): FormGroup {
    return this.fb.group(
      {
        password: [
          '',
          Validators.compose([
            Validators.required,
            
            Validators.minLength(6),
          ]),
        ],
        confirmPassword: ['', Validators.compose([Validators.required])],
      },
      {
        validator: CustomValidators.passwordMatchValidator,
      }
    );
  }

  submitPasswordForm() {
    this.isSubmitting = true;

    // update the model
    this.updateUser(this.resetPasswordForm.value);

    this.profileService.updateProfile(this.user).subscribe(
      (updatedUser) => {
        window.location.href = '/';
      }, //this.router.navigateByUrl('/profile/' + updatedUser.username),
      (err) => {
        this.errors = err;
        this.isSubmitting = false;
      }
    );
  }

  updateUser(values: object) {
    Object.assign(this.user, values);
  }

  togglePassDialog() {
    this.passDialogOpened = !this.passDialogOpened;
    this.resetPasswordForm = this.createPasswordFormGroup();
  }

  //authenticator logic
  twoFATextLogic() {
    if (this.user.hasTwoFactoryAuthEnable === true) {
      this.twoFAStatus = 'Увімкнена';
      this.twoFAButtonText = 'Вимкнути';
    } else {
      this.twoFAStatus = 'Вимкнена';
      this.twoFAButtonText = 'Увімкнути';
    }
  }
  onButtonPrevious() {
    this.currentDialogPage--;
  }
  onButtonNext() {
    this.currentDialogPage++;
  }

  createTwoFAFormGroup(): FormGroup {
    return this.fb.group({
      email: new FormControl({ value: this.user.email, disabled: true }, [
        Validators.required,
      ]),
      password: ['', Validators.required],
    });
  }
  createVerifyTwoFAFormGroup(): FormGroup {
    return this.fb.group({
      email: new FormControl(this.user.email, [Validators.required]),
      password: ['', Validators.required],
      twoFACode: [
        '',
        [
          Validators.required,
          Validators.minLength(6),
          Validators.maxLength(6),
          Validators.pattern('^[0-9]*$'),
        ],
      ],
    });
  }
  createTwoFADisableFormGroup(): FormGroup {
    return this.fb.group({
      email: new FormControl(this.user.email, [Validators.required]),
      password: new FormControl('', [Validators.required]),
      twoFACode: [
        '',
        [
          Validators.required,
          Validators.minLength(6),
          Validators.maxLength(6),
          Validators.pattern('^[0-9]*$'),
        ],
      ],
    });
  }

  toggleTwoFADialog() {
    if (this.user.hasTwoFactoryAuthEnable === true) {
      this.twoFADisableDialogOpened = !this.twoFADisableDialogOpened;
    } else {
      this.twoFAEnableDialogOpened = !this.twoFAEnableDialogOpened;
      this.twoFAForm = this.createTwoFAFormGroup();
      this.verifyTwoFAForm = this.createVerifyTwoFAFormGroup();
    }
  }

  toggleTwoFADisableDialog() {
    this.twoFADisableDialogOpened = !this.twoFADisableDialogOpened;
    // initialize form group with current user data for password and 2FA key fields
  }

  accessTwoFAForm() {
    this.errors = { errors: {} };

    const credentials = this.twoFAForm.getRawValue();
    const request : LoginRequest = {
      email:this.user.email,
      password:this.twoFAForm.value.password
    };
    this.authService.login(request).subscribe(
      (data) => {
        this.currentDialogPage = 2;
      },
      (err) => {
        if (err.status === 401) {
          this.errors = err;
        }
      }
    );
  }

  generate2FA() {
    window.addEventListener('beforeunload', this.twoFAWindowWarning);
    const data : GenearateTwoFactory = { password: this.twoFAForm.value.password, retry: false };
    this.profileService
      .generateTwoFactoryAuth(data)
      .subscribe(({ qrCodeImageURL, manualEntrySetupCode }) => {
        this.twoFAQRCreated = true;
        this.twoFAQRImageURL =
          this.sanitizer.bypassSecurityTrustUrl(qrCodeImageURL);
        this.manualEntrySetupCode = manualEntrySetupCode;
      });
  }

  disable2FA() {
    const credentials : DisableTwoFactoryRequest = {
      password: this.twoFADisableForm.value.password,
      googleAuthCode: this.twoFADisableForm.value.twoFACode,
    };
    this.profileService.disableTwoFactory(credentials).subscribe((data) => {
      this.twoFADisabledSucess = true;
    });
  }

  verifyTwoFAPageInit() {
    this.currentDialogPage = 9;
    this.verifyTwoFAForm = this.createVerifyTwoFAFormGroup();
  }

  twoFAWindowWarning(e : any) {
    e.returnValue = `Please make sure you have saved your 2FA key before leaving this page.`;
  }

  onVerifyTwoFAFormSubmit() {
    this.isSubmitting = true;
    this.errors = { errors: {} };
    const code : VerifyTwoFactoryRequest = { googleAuthCode: this.verifyTwoFAForm.value.twoFACode };
    this.profileService.verifyTwoFactory(code).subscribe(
      () => {
        this.twoFAVerified = true;
        this.twoFAVerificationError = false;
        window.removeEventListener('beforeunload', this.twoFAWindowWarning);
      },
      (err) => {
        if (err.status === 401) {
          this.errors = err;
          this.showInvalidDialog = true;
          this.isSubmitting = false;
          this.twoFAVerificationError = true;
        }
      }
    );
  }

  pageRefresh() {
    window.location.reload();
  }
}
