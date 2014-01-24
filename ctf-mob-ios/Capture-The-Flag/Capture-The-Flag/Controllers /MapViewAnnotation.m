//
//  MapViewAnnotation.m
//  Capture The Flag
//
//  Created by Tomasz Szulc on 16/12/13.
//  Copyright (c) 2013 Tomasz Szulc. All rights reserved.
//

#import "MapViewAnnotation.h"

@implementation MapViewAnnotation

- (id)initWithTitle:(NSString *)title andCoordinate:(CLLocationCoordinate2D)c2d {
    self = [super init];
    if (self) {
        _title = title;
        _coordinate = c2d;
    }
    return self;
}

@end