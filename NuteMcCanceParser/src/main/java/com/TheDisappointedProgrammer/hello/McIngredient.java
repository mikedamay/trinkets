package com.TheDisappointedProgrammer.hello;

public class McIngredient {
    private String shortCode;
    private String name;
    private String description;
    private String group;
    private String previous;
    private String mainDataReference;
    private String footNote;

    public McIngredient(String shortCode, String name, String description,
                        String group, String previous, String mainDataReference, String footNote) {
        this.shortCode = shortCode;
        this.name = name;
        this.description = description;
        this.group = group;
        this.previous = previous;
        this.mainDataReference = mainDataReference;
        this.footNote = footNote;
    }

    public String getShortCode() {
        return shortCode;
    }

    public String getName() {
        return name;
    }

    public String getDescription() {
        return description;
    }

    public String getGroup() {
        return group;
    }

    public String getPrevious() {
        return previous;
    }

    public String getMainDataReference() {
        return mainDataReference;
    }

    public String getFootNote() {
        return footNote;
    }
}
