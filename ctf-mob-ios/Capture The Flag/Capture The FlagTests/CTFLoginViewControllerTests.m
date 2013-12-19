//
//  CTFLoginViewControllerTests.m
//  Capture The Flag
//
//  Created by Tomasz Szulc on 08.11.2013.
//  Copyright (c) 2013 Tomasz Szulc. All rights reserved.
//

#import <XCTest/XCTest.h>
#import "CTFLoginViewController.h"

@interface CTFLoginViewControllerTests : XCTestCase

@end

@implementation CTFLoginViewControllerTests
{
    UIStoryboard *storyboard;
    CTFLoginViewController *vc;
}

- (void)setUp
{
    [super setUp];
    storyboard = [UIStoryboard storyboardWithName:@"LoginAndRegister" bundle:nil];
    vc = [storyboard instantiateViewControllerWithIdentifier:NSStringFromClass([CTFLoginViewController class])];
    [vc view];
}

- (void)tearDown
{
    vc = nil;
    storyboard = nil;
    [super tearDown];
}

- (void)testStoryboardShouldExists
{
    XCTAssertNotNil(storyboard, @"Storyboard should exists");
}

- (void)testCTFLoginVCShouldExists
{
    XCTAssertNotNil(vc, @"Controller with this identifier should exists");
}

#pragma mark - Outlets
- (void)testUserNameTextFieldShouldExists
{
    XCTAssertNotNil(vc.usernameTF, @"");
}

- (void)testPasswordTextFieldShouldExists
{
    XCTAssertNotNil(vc.passwordTF, @"");
}

- (void)testLoginButtonShouldExists
{
    XCTAssertNotNil(vc.loginBtn, @"");
}

- (void)testRegisterButtonShouldExists
{
    XCTAssertNotNil(vc.registerBtn, @"");
}

- (void)testStatusLabelShouldExists
{
    XCTAssertNotNil(vc.statusLabel, @"");
}


#pragma mark - Actions
- (void)testLoginButtonAction
{
    NSString *action = [vc.loginBtn actionsForTarget:vc forControlEvent:UIControlEventTouchUpInside][0];
    XCTAssertEqualObjects(action, @"loginPressed", @"action should be loginPressed");
}


#pragma mark - Segues
- (void)testToRegisterSegue
{
    XCTAssertTrue([vc shouldPerformSegueWithIdentifier:@"ToRegisterSegue" sender:vc], @"");
}

@end