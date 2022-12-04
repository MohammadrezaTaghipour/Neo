Feature: Removing Stream event type

    In order to registering and tracking life strams online
    As me
    I want to configure Stream event types

    Scenario Outline: Stream event type gets removed when it is not used by any life stream
        Given There are some provided stream event type metadata with following properties
            | Title     |
            | Init Date |
        And There is a defined stream event type with following properties
            | Title | Metadata |
            | Init  |          |
        When I Remove stream event type 'Init'
        Then I can not find stream event type 'Init' with above properties

    #TODO: comming soon
    Scenario Outline: Stream event type is not allowed to get removed when it is used by any life stream