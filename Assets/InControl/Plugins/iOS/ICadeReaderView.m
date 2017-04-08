#import "ICadeReaderView.h"


@interface ICadeReaderView()

- (void) didEnterBackground;
- (void) didBecomeActive;

@end


@implementation ICadeReaderView

@synthesize state = _state, delegate = _delegate, active;


- (id) initWithFrame: (CGRect) frame
{
    self = [super initWithFrame: frame];
    inputView = [[UIView alloc] initWithFrame: CGRectZero];

    [[NSNotificationCenter defaultCenter]
        addObserver: self
        selector: @selector(didEnterBackground)
        name:UIApplicationDidEnterBackgroundNotification
        object: nil
    ];

    [[NSNotificationCenter defaultCenter]
        addObserver: self
        selector: @selector(didBecomeActive)
        name: UIApplicationDidBecomeActiveNotification
        object: nil
     ];

    return self;
}


- (void) dealloc
{
    [[NSNotificationCenter defaultCenter]
        removeObserver: self
        name: UIApplicationDidEnterBackgroundNotification
        object: nil
    ];

    [[NSNotificationCenter defaultCenter]
        removeObserver: self
        name: UIApplicationDidBecomeActiveNotification
        object: nil
    ];

    #if !__has_feature(objc_arc)
    [super dealloc];
    #endif
}


- (void) insertText: (NSString *) text
{
    static const char * DN_INPUTS = "wdxayhujikol";
    static const char * UP_INPUTS = "eczqtrfnmpgv";

    bool stateChanged = false;
    char input = [text characterAtIndex: 0];

    for (int i = 0; i < 12; i++)
    {
        if (input == DN_INPUTS[i])
        {
            _state |= (1 << i);
            stateChanged = true;
            break;
        }

        if (input == UP_INPUTS[i])
        {
            _state &= ~(1 << i);
            stateChanged = true;
            break;
        }
    }


    if (stateChanged)
    {
        [_delegate stateChanged: _state];
    }

    static int cycleResponder = 0;
    if (++cycleResponder > 20)
    {
        // necessary to clear a buffer that accumulates internally
        cycleResponder = 0;
        [self resignFirstResponder];
        [self becomeFirstResponder];
    }
}


- (void) didEnterBackground
{
    if (self.active)
    {
        [self resignFirstResponder];
    }
}


- (void) didBecomeActive
{
    if (self.active)
    {
        [self becomeFirstResponder];
    }
}


- (BOOL) canBecomeFirstResponder
{
    return YES;
}


- (void) setActive: (BOOL) value
{
    if (active == value)
    {
        return;
    }

    active = value;

    if (active)
    {
        [self becomeFirstResponder];
    }
    else
    {
        [self resignFirstResponder];
    }
}


- (UIView *) inputView
{
    return inputView;
}


- (BOOL) hasText
{
    return NO;
}


- (void) deleteBackward
{
}


@end


// END

