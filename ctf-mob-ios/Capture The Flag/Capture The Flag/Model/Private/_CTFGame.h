// DO NOT EDIT. This file is machine-generated and constantly overwritten.
// Make changes to CTFGame.h instead.

#import <CoreData/CoreData.h>
#import "CustomManagedObject.h"

extern const struct CTFGameAttributes {
	__unsafe_unretained NSString *created_date;
	__unsafe_unretained NSString *desc;
	__unsafe_unretained NSString *gameId;
	__unsafe_unretained NSString *map;
	__unsafe_unretained NSString *max_players;
	__unsafe_unretained NSString *modified_date;
	__unsafe_unretained NSString *name;
	__unsafe_unretained NSString *start_time;
	__unsafe_unretained NSString *status;
} CTFGameAttributes;

extern const struct CTFGameRelationships {
	__unsafe_unretained NSString *items;
	__unsafe_unretained NSString *players;
} CTFGameRelationships;

extern const struct CTFGameFetchedProperties {
} CTFGameFetchedProperties;

@class CTFItem;
@class CTFUser;




@class NSObject;






@interface CTFGameID : NSManagedObjectID {}
@end

@interface _CTFGame : CustomManagedObject {}
+ (id)insertInManagedObjectContext:(NSManagedObjectContext*)moc_;
+ (NSString*)entityName;
+ (NSEntityDescription*)entityInManagedObjectContext:(NSManagedObjectContext*)moc_;
- (CTFGameID*)objectID;





@property (nonatomic, strong) NSDate* created_date;



//- (BOOL)validateCreated_date:(id*)value_ error:(NSError**)error_;





@property (nonatomic, strong) NSString* desc;



//- (BOOL)validateDesc:(id*)value_ error:(NSError**)error_;





@property (nonatomic, strong) NSNumber* gameId;



@property int32_t gameIdValue;
- (int32_t)gameIdValue;
- (void)setGameIdValue:(int32_t)value_;

//- (BOOL)validateGameId:(id*)value_ error:(NSError**)error_;





@property (nonatomic, strong) id map;



//- (BOOL)validateMap:(id*)value_ error:(NSError**)error_;





@property (nonatomic, strong) NSNumber* max_players;



@property int16_t max_playersValue;
- (int16_t)max_playersValue;
- (void)setMax_playersValue:(int16_t)value_;

//- (BOOL)validateMax_players:(id*)value_ error:(NSError**)error_;





@property (nonatomic, strong) NSDate* modified_date;



//- (BOOL)validateModified_date:(id*)value_ error:(NSError**)error_;





@property (nonatomic, strong) NSString* name;



//- (BOOL)validateName:(id*)value_ error:(NSError**)error_;





@property (nonatomic, strong) NSDate* start_time;



//- (BOOL)validateStart_time:(id*)value_ error:(NSError**)error_;





@property (nonatomic, strong) NSNumber* status;



@property int16_t statusValue;
- (int16_t)statusValue;
- (void)setStatusValue:(int16_t)value_;

//- (BOOL)validateStatus:(id*)value_ error:(NSError**)error_;





@property (nonatomic, strong) NSSet *items;

- (NSMutableSet*)itemsSet;




@property (nonatomic, strong) NSSet *players;

- (NSMutableSet*)playersSet;





@end

@interface _CTFGame (CoreDataGeneratedAccessors)

- (void)addItems:(NSSet*)value_;
- (void)removeItems:(NSSet*)value_;
- (void)addItemsObject:(CTFItem*)value_;
- (void)removeItemsObject:(CTFItem*)value_;

- (void)addPlayers:(NSSet*)value_;
- (void)removePlayers:(NSSet*)value_;
- (void)addPlayersObject:(CTFUser*)value_;
- (void)removePlayersObject:(CTFUser*)value_;

@end

@interface _CTFGame (CoreDataGeneratedPrimitiveAccessors)


- (NSDate*)primitiveCreated_date;
- (void)setPrimitiveCreated_date:(NSDate*)value;




- (NSString*)primitiveDesc;
- (void)setPrimitiveDesc:(NSString*)value;




- (NSNumber*)primitiveGameId;
- (void)setPrimitiveGameId:(NSNumber*)value;

- (int32_t)primitiveGameIdValue;
- (void)setPrimitiveGameIdValue:(int32_t)value_;




- (id)primitiveMap;
- (void)setPrimitiveMap:(id)value;




- (NSNumber*)primitiveMax_players;
- (void)setPrimitiveMax_players:(NSNumber*)value;

- (int16_t)primitiveMax_playersValue;
- (void)setPrimitiveMax_playersValue:(int16_t)value_;




- (NSDate*)primitiveModified_date;
- (void)setPrimitiveModified_date:(NSDate*)value;




- (NSString*)primitiveName;
- (void)setPrimitiveName:(NSString*)value;




- (NSDate*)primitiveStart_time;
- (void)setPrimitiveStart_time:(NSDate*)value;




- (NSNumber*)primitiveStatus;
- (void)setPrimitiveStatus:(NSNumber*)value;

- (int16_t)primitiveStatusValue;
- (void)setPrimitiveStatusValue:(int16_t)value_;





- (NSMutableSet*)primitiveItems;
- (void)setPrimitiveItems:(NSMutableSet*)value;



- (NSMutableSet*)primitivePlayers;
- (void)setPrimitivePlayers:(NSMutableSet*)value;


@end
