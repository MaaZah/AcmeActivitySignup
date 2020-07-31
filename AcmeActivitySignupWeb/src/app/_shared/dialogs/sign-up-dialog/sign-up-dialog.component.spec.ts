import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { SignUpDialogComponent } from './sign-up-dialog.component';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { of } from 'rxjs';
import { ActivityDetails } from '../../models/activity-details';
import { ApiService } from '../../services/api.service';

describe('SignUpDialogComponent', () => {
  let component: SignUpDialogComponent;
  let fixture: ComponentFixture<SignUpDialogComponent>;

  let apiServiceStub: any;

  beforeEach(async(() => {

    //fake activities for service stub
    let fakeActivities = [new ActivityDetails(1, "test", "test", new Date(), "test"), new ActivityDetails(2, "test", "test", new Date(), "test")];
    apiServiceStub = {
      GetActivitiesList: () => of(fakeActivities)
    };

    TestBed.configureTestingModule({
      declarations: [ SignUpDialogComponent ],
      imports: [HttpClientTestingModule, MatDialogModule, FormsModule, ReactiveFormsModule, RouterTestingModule.withRoutes([])],
      providers: [
        { provide: MAT_DIALOG_DATA, useValue: {} },
        { provide: MatDialogRef, useValue: {} },
        { provide: ApiService, useValue: apiServiceStub}
    ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SignUpDialogComponent);
    component = fixture.componentInstance;
    component.ngOnInit();
    fixture.detectChanges();
    
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render input elements', () => {
    const compiled = fixture.debugElement.nativeElement;
    const firstNameInput = compiled.querySelector('input[formControlName="firstName"]');
    const lastNameInput = compiled.querySelector('input[formControlName="lastName"]');
    const emailInput = compiled.querySelector('input[formControlName="email"]');
    const commentsInput = compiled.querySelector('input[formControlName="comments"]');
    const activityInput = compiled.querySelector('select[formControlName="activity"]');

    expect(firstNameInput).toBeTruthy();
    expect(lastNameInput).toBeTruthy();
    expect(emailInput).toBeTruthy();
    expect(commentsInput).toBeTruthy();
    expect(activityInput).toBeTruthy();
  });


  it('should test input validity', () => {
    const firstNameInput = component.signupForm.controls.firstName;
    const lastNameInput = component.signupForm.controls.lastName;
    const emailInput = component.signupForm.controls.email;

    //empty inputs fail required
    expect(firstNameInput.valid).toBeFalsy();
    expect(lastNameInput.valid).toBeFalsy();
    expect(emailInput.valid).toBeFalsy();

    //valid inputs
    firstNameInput.setValue("Test");
    lastNameInput.setValue("Test");
    emailInput.setValue("validemail@test.ca");

    //valid inputs pass
    expect(firstNameInput.valid).toBeTruthy();
    expect(lastNameInput.valid).toBeTruthy();
    expect(emailInput.valid).toBeTruthy();

    //invalid email
    emailInput.setValue("invalidemail");

    //invalid email fails
    expect(emailInput.valid).toBeFalsy();
  });
});
