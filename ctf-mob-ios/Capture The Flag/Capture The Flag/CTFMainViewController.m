//
//  CTFMainViewController.m
//  Capture The Flag
//
//  Created by Tomasz Szulc on 18.11.2013.
//  Copyright (c) 2013 Tomasz Szulc. All rights reserved.
//

#import "CTFMainViewController.h"
#import "CTFGame.h"
#import "CTFUser.h"

@interface CTFMainViewController ()

@end

@implementation CTFMainViewController

- (void)viewDidLoad
{
    [super viewDidLoad];
	// Do any additional setup after loading the view.
}

- (void)viewWillAppear:(BOOL)animated
{
    [super viewWillAppear:animated];
    
    CTFUser *user = [CTFGame sharedInstance].currentUser;
    if (user) {
        self.navigationItem.title = [NSString stringWithFormat:@"@%@",user.username];
    }
}

- (IBAction)onLogout:(id)sender {
    [CTFGame setSharedInstance:nil];
    [self dismissViewControllerAnimated:YES completion:nil];
}

@end
