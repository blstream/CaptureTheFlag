//
//  CTFRegisterViewController.m
//  Capture The Flag
//
//  Created by Tomasz Szulc on 07/11/13.
//  Copyright (c) 2013 Tomasz Szulc. All rights reserved.
//

#import "CTFRegisterViewController.h"

#import "CTFAPIConnection.h"
#import "CTFAPILocalCredentialsValidator.h"
#import "CTFAPIAccounts.h"

@interface CTFRegisterViewController () <UIAlertViewDelegate>
@end

@implementation CTFRegisterViewController {
    CTFAPIAccounts *_accounts;
    UIAlertView *_successAlert;
    UIAlertView *_failureAlert;
}

#pragma mark - Lifecycle

- (void)viewDidLoad {
    [super viewDidLoad];
    [self _configureTapBackground];
    [self _configureTextFields];
}

- (void)viewDidUnload {
    _successAlert = nil;
    _failureAlert = nil;
    _accounts = nil;
    [super viewDidUnload];
}

- (void)_configureTapBackground {
    UITapGestureRecognizer *gesture = [[UITapGestureRecognizer alloc] initWithTarget:self action:@selector(_backgroundTapped)];
    [self.view addGestureRecognizer:gesture];
}

- (void)_backgroundTapped {
    [self.view endEditing:YES];
}

- (IBAction)registerPressed {
    if (![_passwordTF.text isEqualToString:_rePasswordTF.text]) {
        _statusLabel.text = NSLocalizedString(@"view.register.label.status.different_password", nil);
        return;
    }
    [self _setUIEnabled:NO];
    [_activityIndicator startAnimating];
    _accounts = [[CTFAPIAccounts alloc] initWithConnection:[CTFAPIConnection sharedConnection]];
    [_accounts signUpWithUsername:_usernameTF.text email:_emailTF.text password:_passwordTF.text block:^(BOOL success) {
        
        NSString *title = NSLocalizedString(@"view.register.alert.registration.title", nil);
        NSString *message = @"";
        if (success) {
            message = NSLocalizedString(@"view.register.alert.registration.message.success", nil);
        } else {
            message = NSLocalizedString(@"view.register.alert.registration.message.failure", nil);
        }
        UIAlertView *alertView =
        [[UIAlertView alloc] initWithTitle:title
                                   message:message
                                  delegate:Nil
                         cancelButtonTitle:NSLocalizedString(@"button.OK", nil)
                         otherButtonTitles:nil, nil];
        alertView.delegate = self;
        
        if (success) {
            _successAlert = alertView;
            [_successAlert show];
        } else {
#warning [tsu] think about error codes. What code will be returned (400?) if user already exists? There will be need to add handling for server error codes in response descriptors. Maybe block should be improved.
            _failureAlert = alertView;
            [_failureAlert show];
        }
        
        [_activityIndicator stopAnimating];
        [self _setUIEnabled:YES];
    }];
}

- (void)_setUIEnabled:(BOOL)state {
    [_emailTF setEnabled:state];
    [_usernameTF setEnabled:state];
    [_passwordTF setEnabled:state];
    [_rePasswordTF setEnabled:state];
    [_registerBtn setEnabled:state];
}

- (void)_configureTextFields {
    [_emailTF addTarget:self action:@selector(textFieldDidChange) forControlEvents:UIControlEventEditingChanged];
    [_usernameTF addTarget:self action:@selector(textFieldDidChange) forControlEvents:UIControlEventEditingChanged];
    [_passwordTF addTarget:self action:@selector(textFieldDidChange) forControlEvents:UIControlEventEditingChanged];
    [_rePasswordTF addTarget:self action:@selector(textFieldDidChange) forControlEvents:UIControlEventEditingChanged];
}

- (void)textFieldDidChange {
    BOOL registrationEnabled = NO;
    CredentialsValidationResult result =
    [CTFAPILocalCredentialsValidator validateSignUpCredentialsWithUsername:_usernameTF.text
                                             emailAddress:_emailTF.text
                                                 password:_passwordTF.text
                                               rePassword:_rePasswordTF.text];
    
    if (result == CredentialsValidationResultOK) {
        registrationEnabled = YES;
    }
    
    [_registerBtn setEnabled:registrationEnabled];
}


#pragma mark - UITextFieldDelegate
- (BOOL)textFieldShouldReturn:(UITextField *)textField {
    [textField resignFirstResponder];
    return YES;
}


#pragma mark - UIAlertViewDelegate
- (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex {
    if ([alertView isEqual:_successAlert] && buttonIndex == 0) {
        [self.navigationController popViewControllerAnimated:YES];
    }
}


#pragma mark - CTFViewControllerProtocol
- (void)localizeUI {
    self.navigationItem.title = NSLocalizedString(@"view.register.navigation.title", nil);
    _emailTF.placeholder = NSLocalizedString(@"view.register.textField.email.placeholder", nil);
    _usernameTF.placeholder = NSLocalizedString(@"view.register.textField.username.placeholder", nil);
    _passwordTF.placeholder = NSLocalizedString(@"view.register.textField.password.placeholder", nil);
    _rePasswordTF.placeholder = NSLocalizedString(@"view.register.textField.re-password.placeholder", nil);
    [_registerBtn setTitle:NSLocalizedString(@"view.register.button.register.title", nil) forState:UIControlStateNormal];
}

@end
