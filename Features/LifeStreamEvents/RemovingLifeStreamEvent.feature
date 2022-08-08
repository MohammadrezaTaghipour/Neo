Feature: Removing life stream event

    In order to registering and tracking life strams online
    As me
    I want to configure life stream events

    Scenario Outline: Life stream event gets removed when it is not used by any life stream
        Given There are some provided life stream event metadata with following properties
            | Title     |
            | Init Date |
        And There is a defined life stream event with title with following properties
            | Title | Metadata |
            | Init  |          |
        When I Remove life stream event 'Init'
        Then Then I can not find life stream event 'Init' with above properties

    #TODO: comming soon
    Scenario Outline: Life stream event is not allowed to get removed when it is used by any life stream