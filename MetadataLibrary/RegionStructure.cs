using System;
using System.Collections.Generic;
using System.Drawing;
//using System.Runtime.InteropServices.WindowsRuntime;

namespace MetadataLibrary
{
    #region rotate_regions.config
    /*
        #------------------------------------------------------------------------------
        # File:         rotate_regions.config
        #
        # Description:  User-defined Composite tag definitions to rotate MWG region tags
        #               (Metadata Working Group region, used by Picasa) and MP region tags
        #               (used by Microsoft Photo Library).
        #
        # Tag definitions and examples:
        #
        #   RotateMWGRegionCW90
        #   RotateMWGRegionCW180
        #   RotateMWGRegionCW270
        #     These tags will rotate a MWG Region clockwise 90, 180, or 270 degrees.
        #     Example:
        #       exiftool -config rotate_regions.config "-RegionInfo<RotateMWGRegionCW90" FILE
        #
        #   RotateMPRegionCW90
        #   RotateMPRegionCW180
        #   RotateMPRegionCW270
        #     These tags will rotate a MWG Region clockwise 90, 180, or 270 degrees.
        #     Example:
        #       exiftool -config rotate_regions.config "-RegionInfoMP<RotateMPRegionCW90" FILE
        #
        # Revisions:    2015/05/08 - Bryan K. Williams AKA StarGeek Created
        #------------------------------------------------------------------------------

        %Image::ExifTool::UserDefined = (
            'Image::ExifTool::Composite' => {
                RotateMWGRegionCW90 =>{
                    Require => 'RegionInfo',
                    ValueConv => q{
                        my ($rgn, @newRgns);
                        foreach $rgn (@{$val[0]{RegionList}}) {
                            my @rect = @{$$rgn{Area}}{'X','Y','W','H'};
                            my %newRgn = (
                                Area => {
                                    X => 1-$rect[1],
                                    Y => $rect[0],
                                    W => $rect[3],
                                    H => $rect[2],
                                    Unit => 'normalized',
                                },
                                Name => $$rgn{Name},
                                Type => 'Face',
                            );
                            push @newRgns, \%newRgn;
                        }
                        return {
                            AppliedToDimensions => {
                                W => $val[0]{AppliedToDimensions}{W},
                                H => $val[0]{AppliedToDimensions}{H},
                                Unit => $val[0]{AppliedToDimensions}{Unit},
                            },
                            RegionList => \@newRgns,
                        };
                    },
                }, #End RotateMWGRegionCW90
                RotateMWGRegionCW180 =>{
                    Require => 'RegionInfo',
                    ValueConv => q{
                        my ($rgn, @newRgns);
                        foreach $rgn (@{$val[0]{RegionList}}) {
                            my @rect = @{$$rgn{Area}}{'X','Y','W','H'};
                            my %newRgn = (
                                Area => {
                                    X => 1-$rect[0],
                                    Y => 1-$rect[1],
                                    W => $rect[2],
                                    H => $rect[3],
                                    Unit => 'normalized',
                                },
                                Name => $$rgn{Name},
                                Type => 'Face',
                            );
                            push @newRgns, \%newRgn;
                        }
                        return {
                            AppliedToDimensions => {
                                W => $val[0]{AppliedToDimensions}{W},
                                H => $val[0]{AppliedToDimensions}{H},
                                Unit => $val[0]{AppliedToDimensions}{Unit},
                            },
                            RegionList => \@newRgns,
                        };
                    },
                }, #End RotateMWGRegionCW180
                RotateMWGRegionCW270 =>{
                    Require => 'RegionInfo',
                    ValueConv => q{
                        my ($rgn, @newRgns);
                        foreach $rgn (@{$val[0]{RegionList}}) {
                            my @rect = @{$$rgn{Area}}{'X','Y','W','H'};
                            my %newRgn = (
                                Area => {
                                    X => $rect[1],
                                    Y => 1-$rect[0],
                                    W => $rect[3],
                                    H => $rect[2],
                                    Unit => 'normalized',
                                },
                                Name => $$rgn{Name},
                                Type => 'Face',
                            );
                            push @newRgns, \%newRgn;
                        }
                        return {
                            AppliedToDimensions => {
                                W => $val[0]{AppliedToDimensions}{W},
                                H => $val[0]{AppliedToDimensions}{H},
                                Unit => $val[0]{AppliedToDimensions}{Unit},
                            },
                            RegionList => \@newRgns,
                        };
                    },
                }, #End RotateMWGRegionCW270
                RotateMPRegionCW90=>{
                    Require => 'RegionInfoMP',
                    ValueConv => q{
                        my ($rgn, @newRgns);
                        foreach $rgn (@{$val[0]{Regions}}) {
                            my @rect = split /\s*,\s* /, $$rgn{Rectangle
            };
            my $temp = $rect[0];
                            $rect[0] = 1-$rect[1]-$rect[3];
                            $rect[1] = $temp;
                            ($rect[2], $rect[3]) = ($rect[3],$rect[2]);  #Swap W and H
                            push @newRgns, {
                                PersonDisplayName => $$rgn{PersonDisplayName
        },
                                Rectangle => join(', ', @rect),
                            };
                        }
                        return { Regions => \@newRgns };
                    }
                }, #end RotateMPRegionCW90
                RotateMPRegionCW180=>{
                    Require => 'RegionInfoMP',
                    ValueConv => q{
                        my($rgn, @newRgns);
                        foreach $rgn(@{$val[0]{ Regions} }) {
                            my @rect = split /\s*,\s* /, $$rgn{Rectangle};
                            my $tempX = $rect[0];
                            my $tempY = $rect[1];
                            $rect[0] = 1-$tempX-$rect[2];
                            $rect[1] = 1-$tempY-$rect[3];
                            push @newRgns, {
                                PersonDisplayName => $$rgn{PersonDisplayName},
                                Rectangle => join(', ', @rect),
                            };
                        }
                        return { Regions => \@newRgns };
                    }
                }, #end RotateMPRegionCW180
                RotateMPRegionCW270=>{
                    Require => 'RegionInfoMP',
                    ValueConv => q{
                        my($rgn, @newRgns);
                        foreach $rgn(@{$val[0]{ Regions} }) {
                            my @rect = split /\s*,\s* /, $$rgn{Rectangle};
                            my $temp = $rect[1];
                            $rect[1] = 1-$rect[0]-$rect[2];
                            $rect[0] = $temp;
                            ($rect[2], $rect[3]) = ($rect[3],$rect[2]);  #Swap W and H
                            push @newRgns, {
                                PersonDisplayName => $$rgn{PersonDisplayName},
                                Rectangle => join(', ', @rect),
                            };
                        }
                        return { Regions => \@newRgns };
                    }
                }, #end RotateMPRegionCW270
            },
        );
          
        */
    #endregion

    #region convert_regions.config    
    /*

    #------------------------------------------------------------------------------
    # File:         convert_regions.config
    #
    # Description:  User-defined Composite tag definitions to allow conversion of
    #               face regions between Microsoft Windows Live Photo Gallery (WLPG),
    #               Metadata Working Group (MWG), and IPTC Extension formats
    #
    # Usage:     1) Convert from MP WLPG or IPTC regions to MWG regions:
    #               exiftool -config convert_regions.config "-regioninfo<myregion" FILE
    #
    #            2) Convert from MWG or IPTC to MP WLPG regions:
    #               exiftool -config convert_regions.config "-regioninfomp<myregionmp" FILE
    #
    #            3) Convert from MWG or MP WLPG to IPTC regions:
    #               exiftool -config convert_regions.config "-imageregion<myregioniptc" FILE
    #
    #               2019/10/26 - PH Added support for the new IPTC ImageRegion
    # References:   http://www.metadataworkinggroup.org/specs/
    #------------------------------------------------------------------------------

    %Image::ExifTool::UserDefined = (

        'Image::ExifTool::Composite' => {

            # create an MWG RegionInfo structure from a Microsoft RegionInfoMP structure
            MyRegion => {
                Require => {
                    0 => 'RegionInfoMP',
                    1 => 'ImageWidth',
                    2 => 'ImageHeight',
                },
                ValueConv => q{
                    my ($rgn, @newRgns);
                    foreach $rgn (@{$val[0]{Regions}}) {
                        my $name = $$rgn{PersonDisplayName};
                        next unless $$rgn{Rectangle} or defined $name;
                        my %newRgn = ( Type => 'Face' );
                        if (defined $name) {
                            # don't add ignored faces
                            next if $name eq 'ffffffffffffffff';
                            $newRgn{Name} = $name;
                        }
                        if ($$rgn{Rectangle}) {
                            my @rect = split /\s*,\s* /, $$rgn{Rectangle
        };
                            $newRgn{Area
    } = {
                                X => $rect[0] + $rect[2]/2,
                                Y => $rect[1] + $rect[3]/2,
                                W => $rect[2],
                                H => $rect[3],
                                Unit => 'normalized',
                            } if @rect == 4;
                        }
                        push @newRgns, \%newRgn;
                    }
                    return {
                        AppliedToDimensions => { W => $val[1], H => $val[2], Unit => 'pixel' },
                        RegionList => \@newRgns,
                    };
                },
            },

            # create an MWG RegionInfo structure from an IPTC ImageRegion list
            MyRegion2 => {
                Name => 'MyRegion',
                Require => {
                    0 => 'ImageRegion',
                    1 => 'ImageWidth',
                    2 => 'ImageHeight',
                },
                ValueConv => q{
                    my($rgn, @newRgns);
                    my $rgns = ref $val[0] eq 'ARRAY' ? $val[0] : [ $val[0] ]; 
                                    foreach $rgn(@$rgns)
                    {
                        my % newRgn = (Type => 'Face');
                        if ($$rgn{ RegionBoundary}
                        and $$rgn{ RegionBoundary}
                        { RbShape}
                        eq 'rectangle') {
                            my @rect = @{$$rgn{ RegionBoundary} }
                            { 'RbX','RbY','RbW','RbH'};
                            if ($$rgn{ RegionBoundary}
                            { RbUnit}
                            eq 'pixel') {
                                                $rect[0] /= $val[1],  $rect[2] /= $val[1];
                                                $rect[1] /= $val[2];  $rect[3] /= $val[2];
                            }
                                            $newRgn{ Area} = {
                                                X => $rect[0] + $rect[2] / 2,
                                                Y => $rect[1] + $rect[3] / 2,
                                                W => $rect[2],
                                                H => $rect[3],
                                                Unit => 'normalized',
                                            };
                        } else
                        {
                            next unless defined $$rgn{ Name};
                        }
                                        $newRgn{ Name} = $$rgn{ Name}
                        if defined $$rgn{ Name};
                        push @newRgns, \% newRgn;
                    }
                    return {
                        AppliedToDimensions => { W => $val[1], H => $val[2], Unit => 'pixel' },
                        RegionList => \@newRgns,
                    };
                },
            },

            # create a Microsoft RegionInfoMP structure from an MWG RegionInfo structure
            MyRegionMP => {
                Require => 'RegionInfo',
                ValueConv => q{
                    my($rgn, @newRgns);
                    foreach $rgn(@{$val[0]{ RegionList} }) {
                        next unless $$rgn{Area} or defined $$rgn{Name};
                        my %newRgn;
                        if ($$rgn{Area}) {
                            my @rect = @{$$rgn{Area}}{'X','Y','W','H'};
                            $rect[0] -= $rect[2]/2;
                            $rect[1] -= $rect[3]/2;
                            $newRgn{Rectangle} = join(', ', @rect);
                        }
                        $newRgn{PersonDisplayName} = $$rgn{Name} if defined $$rgn{Name};
                        push @newRgns, \%newRgn;
                    }
                    return { Regions => \@newRgns };
                },
            },

            # create a Microsoft RegionInfoMP structure from an IPTC ImageRegion list
            MyRegionMP2 => {
                Name => 'MyRegionMP',
                Require => {
                    0 => 'ImageRegion',
                    1 => 'ImageWidth',
                    2 => 'ImageHeight',
                },
                ValueConv => q{
                    my($rgn, @newRgns);
                    my $rgns = ref $val[0] eq 'ARRAY' ? $val[0] : [ $val[0] ]; 
                                    foreach $rgn(@$rgns)
                    {
                        my % newRgn;
                        if ($$rgn{ RegionBoundary}
                        and $$rgn{ RegionBoundary}
                        { RbShape}
                        eq 'rectangle') {
                            my @rect = @{$$rgn{ RegionBoundary} }
                            { 'RbX','RbY','RbW','RbH'};
                            if ($$rgn{ RegionBoundary}
                            { RbUnit}
                            eq 'pixel') {
                                                $rect[0] /= $val[1],  $rect[2] /= $val[1];
                                                $rect[1] /= $val[2];  $rect[3] /= $val[2];
                            }
                                            $newRgn{ Rectangle} = join(', ', @rect);
                        } else
                        {
                            next unless defined $$rgn{ Name};
                        }
                                        $newRgn{ PersonDisplayName} = $$rgn{ Name}
                        if defined $$rgn{ Name};
                        push @newRgns, \% newRgn;
                    }
                    return { Regions => \@newRgns };
                },
            },

            # create an IPTC ImageRegion list from an MWG RegionInfo structure
            MyRegionIPTC => {
                Require => 'RegionInfo',
                ValueConv => q{
                    my($rgn, @newRgns);
                    foreach $rgn(@{$val[0]{ RegionList} }) {
                        next unless $$rgn{Area} or defined $$rgn{Name};
                        my %newRgn;
                        if ($$rgn{Area}) {
                            my @rect = @{$$rgn{Area}}{'X','Y','W','H'};
                            $rect[0] -= $rect[2]/2;
                            $rect[1] -= $rect[3]/2;
                            $newRgn{RegionBoundary} = {
                                RbShape => 'rectangle',
                                RbUnit => 'relative',
                                RbX => $rect[0],
                                RbY => $rect[1],
                                RbW => $rect[2],
                                RbH => $rect[3],
                            };
                        }
                        $newRgn{Name} = $$rgn{Name} if defined $$rgn{Name};
                        push @newRgns, \%newRgn;
                    }
                    return \@newRgns;
                },
            },

            # create an IPTC ImageRegion list from a Microsoft RegionInfoMP structure
            MyRegionIPTC2 => {
                Name => 'MyRegionIPTC',
                Require => 'RegionInfoMP',
                ValueConv => q{
                    my ($rgn, @newRgns);
                    foreach $rgn (@{$val[0]{Regions}}) {
                        my $name = $$rgn{PersonDisplayName};
                        next unless $$rgn{Rectangle} or defined $name;
                        my %newRgn;
                        if (defined $name) {
                            # don't add ignored faces
                            next if $name eq 'ffffffffffffffff';
                            $newRgn{Name} = $name;
                        }
                        if ($$rgn{Rectangle}) {
                            my @rect = split /\s*,\s* /, $$rgn{Rectangle};
                            $newRgn{RegionBoundary} = {
                                RbShape => 'rectangle',
                                RbUnit => 'relative',
                                RbX => $rect[0],
                                RbY => $rect[1],
                                RbW => $rect[2],
                                RbH => $rect[3],
                            } if @rect == 4;
                        }
                        push @newRgns, \%newRgn;
                    }
                    return \@newRgns;
                },
            },
        },
    );

    1;  #end
    */
    #endregion

    [Serializable]
    public class RegionStructure //: IEquatable<RegionStructure>
    {
        private String type;           
        private float areaX;           
        private float areaY;           
        private float areaWidth;       
        private float areaHeight;      
        private String name;
        private RegionStructureTypes regionStructureType;
        private Image thumbnail;
        private static float acceptRegionMissmatchProcent = 0.3F;


        public string Type { get => type; set => type = value; }
        public float AreaX
        {
            get => areaX;
            set => areaX = (float)Math.Round((float)value, SqliteDatabase.SqliteDatabaseUtilities.NumberOfDecimals);
        }
        public float AreaY 
        {
            get => areaY;
            set => areaY = (float)Math.Round((float)value, SqliteDatabase.SqliteDatabaseUtilities.NumberOfDecimals);
        }
        public float AreaWidth 
        { 
            get => areaWidth; 
            set => areaWidth = (float)Math.Round((float)value, SqliteDatabase.SqliteDatabaseUtilities.NumberOfDecimals);
        }
        public float AreaHeight 
        { 
            get => areaHeight; 
            set => areaHeight = (float)Math.Round((float)value, SqliteDatabase.SqliteDatabaseUtilities.NumberOfDecimals);
        }
        public string Name { get => name; set => name = value; }
        public RegionStructureTypes RegionStructureType { get => regionStructureType; set => regionStructureType = value; }
        public Image Thumbnail { get => thumbnail; set => thumbnail = value; }

        public override int GetHashCode()
        {            
            return 1236563 +
                 Type.GetHashCode() +
                 Name.GetHashCode() +
                 RegionStructureType.GetHashCode() +
                 GetAbstractRectangle().GetHashCode();
        }

        public RegionStructure()
        {
        }

        public RegionStructure(RegionStructure region)
        {
            try
            {
                Type = region.type;
                AreaX = region.areaX;
                AreaY = region.areaY;
                AreaWidth = region.areaWidth;
                AreaHeight = region.areaHeight;
                Name = region.name;
                RegionStructureType = region.regionStructureType;
                Thumbnail = region.thumbnail == null ? null : new Bitmap(region.thumbnail); //DEBUG: This is still not thread safe, need fix
            }
            catch { } //DEBUG: This is still not thread safe, need fix
        }

        public RegionStructure(string name, string type, float areaX, float areaY, float areaWidth, float areaHeight, RegionStructureTypes regionStructureType)
        {
            Name = name;
            Type = type;
            AreaX = areaX;
            AreaY = areaY;
            AreaWidth = areaWidth;
            AreaHeight = areaHeight;
            RegionStructureType = regionStructureType;
        }

        private Rectangle GetAbstractRectangle()
        {
            Size abstractSize = new Size(100000, 100000);
            Rectangle rectangle = this.GetImageRegionPixelRectangle(abstractSize);
            rectangle.X = (int)Math.Round((rectangle.X / 10F));
            rectangle.Y = (int)Math.Round((rectangle.Y / 10F));
            rectangle.Width = (int)Math.Round((rectangle.Width / 10F));
            rectangle.Height = (int)Math.Round((rectangle.Height / 10F));
            return rectangle;
        }
        public static void SetAcceptRegionMissmatchProcent (float newAcceptRegionMissmatchProcent)
        {
            acceptRegionMissmatchProcent = newAcceptRegionMissmatchProcent;
        }

        public static bool RectangleEqual(Rectangle mediaRectangleInput, Rectangle mediaRectangleCell)
        {
            return Math.Abs(mediaRectangleInput.X - mediaRectangleCell.X) < (Math.Max(mediaRectangleInput.Width, mediaRectangleCell.Width) * acceptRegionMissmatchProcent) && //Allow 20% diffrent i region size
                Math.Abs(mediaRectangleInput.Y - mediaRectangleCell.Y) < (Math.Max(mediaRectangleInput.Height, mediaRectangleCell.Height) * acceptRegionMissmatchProcent) &&
                Math.Abs(mediaRectangleInput.Height - mediaRectangleCell.Height) < (Math.Max(mediaRectangleInput.Height, mediaRectangleCell.Height) * acceptRegionMissmatchProcent) &&
                Math.Abs(mediaRectangleInput.Width - mediaRectangleCell.Width) < (Math.Max(mediaRectangleInput.Width, mediaRectangleCell.Width) * acceptRegionMissmatchProcent);
        }

        private static bool NameEqual(string stringThis, string stringCell)
        {
            if (string.IsNullOrEmpty(stringCell) && !string.IsNullOrEmpty(stringThis)) return false; //One name empty other not
            if (!string.IsNullOrEmpty(stringCell) && string.IsNullOrEmpty(stringThis)) return false; //One name empty other not            
            if (string.IsNullOrEmpty(stringCell) && string.IsNullOrEmpty(stringThis)) return true; //Both name empty, return equal
            return stringCell.Trim() == stringThis.Trim();
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false; // If parameter is null, return false.
            if (Object.ReferenceEquals(this, obj)) return true; // Optimization for a common success case.
            if (!(obj is RegionStructure)) return false;

            // Return true if the fields match.
            if (this.RegionStructureType != (obj as RegionStructure).RegionStructureType) return false;
            if (!RegionStructure.RectangleEqual(GetAbstractRectangle(), (obj as RegionStructure).GetAbstractRectangle())) return false;
            if (!NameEqual(this.Name, (obj as RegionStructure).Name)) return false;
            return true;
        }

        public static bool operator ==(RegionStructure left, RegionStructure right)
        {
            if (left is null) return right is null; //If class, need this back
            return left.Equals(right);
        }


        public static bool operator !=(RegionStructure c1, RegionStructure c2)
        {
            return !(c1 == c2);
        }

        public bool DoesThisRectangleAndNameExistInList(List<RegionStructure> regionStructures)
        {
            foreach (RegionStructure regionStructure in regionStructures)
            {
                if (RegionStructure.RectangleEqual(GetAbstractRectangle(), regionStructure.GetAbstractRectangle()) && NameEqual(Name, regionStructure.Name)) return true;
            }
            return false;
        }

        public int IndexOfRectangleInList(List<RegionStructure> regionStructures)
        {
            for (int index = 0; index < regionStructures.Count; index++)
            {
                if (RegionStructure.RectangleEqual(GetAbstractRectangle(), regionStructures[index].GetAbstractRectangle()) ) return index;
            }
            return -1;
        }

        public int IndexOfNameInList(List<RegionStructure> regionStructures)
        {
            for (int index = 0; index < regionStructures.Count; index++)
            {
                if (name == regionStructures[index].Name) return index;
            }
            return -1;
        }

        public bool DoesThisNameExistInList(List<RegionStructure> regionStructures)
        {
            foreach (RegionStructure regionStructure in regionStructures)
            {
                if (NameEqual(Name, regionStructure.Name)) return true;
            }
            return false;
        }

        public static bool DoesThisNameExistInList(List<RegionStructure> regionStructures, string name)
        {
            foreach (RegionStructure regionStructure in regionStructures)
            {
                if (NameEqual(name, regionStructure.Name)) return true;
            }
            return false;
        }

        public bool ShowNameInToString { get; set; } = false;
        public override string ToString()
        {
            return (ShowNameInToString ? name ?? "" : "");
        }

        public string ToStringDebug()
        {
            return "Name: " + (name ?? "(Unknown)") + "\r\n" +
                " Type: " + Type + " Source: " + RegionStructureType + "\r\n" +
                " Area X: " + AreaX + " Y: " + AreaY + " W: " + AreaWidth + " H: " + AreaHeight + "\r\n";
        }
        public string ToolTipText(Size imageSize)
        {
            Rectangle rectangle = RegionStructure.CalculateImageRegionPixelRectangle(RegionStructureType, imageSize, this.GetRegionAbstractRectangle());
            return "Name: " + (name ?? "(Unknown)") + "\r\n" +
                " Type: " + Type + " Source: " + RegionStructureType + "\r\n" +
                " X: " + rectangle.X + " Y: " + rectangle.Y + " W: " + rectangle.Width + " H: " + rectangle.Height + "\r\n" +
                " X: " + AreaX + " Y: " + AreaY + " W: " + AreaWidth + " H: " + AreaHeight + "\r\n";
        }

        public string RegionErrorText(Size imageSize)
        {
            Rectangle rectangle = RegionStructure.CalculateImageRegionPixelRectangle(RegionStructureType, imageSize, this.GetRegionAbstractRectangle());
            return "Name: " + (name ?? "(Unknown)") + "\r\n" +
                "Type: " + Type + "\r\n" +
                "Source: " + RegionStructureType + "\r\n" +
                "Using size: W: " + imageSize.Width + " H: " + imageSize.Width + "\r\n" +
                "X: " + rectangle.X + " Y: " + rectangle.Y + " W: " + rectangle.Width + " H: " + rectangle.Height + "\r\n";
        }

        public string ToErrorText()
        {
            return "Name: " + (string.IsNullOrEmpty(name) ? "(Unknown)" : name) + " Type: " + Type + " Source: " + RegionStructureType + " X: " + AreaX + " Y: " + AreaY + " W: " + AreaWidth + " H: " + AreaHeight;
        }

        private RectangleF GetRegionAbstractRectangle()
        {
            return new RectangleF((float)AreaX, (float)AreaY, (float)AreaWidth, (float)AreaHeight);
        }

        private static Size CalculateImageRegionPixelSize (RegionStructureTypes regionStructureType, Size imagePixelSize, SizeF regionAreaAbstractSize)
        {
            Size sizeFace;
            switch (regionStructureType)
            {
                case RegionStructureTypes.IptcRelative:
                case RegionStructureTypes.WindowsLivePhotoGallery:
                case RegionStructureTypes.WindowsLivePhotoGalleryDatabase:
                    sizeFace = new Size(
                        (int)((float)imagePixelSize.Width * regionAreaAbstractSize.Width) * 1,
                        (int)((float)imagePixelSize.Height * regionAreaAbstractSize.Height) * 1);
                    break;

                case RegionStructureTypes.MetadataWorkingGroup:
                    sizeFace = new Size(
                        (int)((float)imagePixelSize.Width * regionAreaAbstractSize.Width * 1),
                        (int)((float)imagePixelSize.Height * regionAreaAbstractSize.Height * 1)
                        );
                    break;
                case RegionStructureTypes.IptcPixel: //Not testeded and this means not implemented
                    sizeFace = new Size( (int)regionAreaAbstractSize.Width, (int)regionAreaAbstractSize.Height);
                    throw new Exception("Not implemented");
                    //break;
                default:
                    sizeFace = new Size(
                        (int)((float)imagePixelSize.Width * regionAreaAbstractSize.Width) * 1,
                        (int)((float)imagePixelSize.Height * regionAreaAbstractSize.Height) * 1);
                    //throw new Exception("Not implemented");
                    break;
            }

            //In case of wrong size in EXIF data, check size and make sure not copy outside allocateded picture
            if (sizeFace.Width < 1 || sizeFace.Width > imagePixelSize.Width) sizeFace.Width = imagePixelSize.Width;
            if (sizeFace.Height < 1 || sizeFace.Height > imagePixelSize.Height) sizeFace.Height = imagePixelSize.Height;
            return sizeFace;
        }

        

        public Rectangle GetImageRegionPixelRectangle(Size imagePixelSize)
        {
            return CalculateImageRegionPixelRectangle(RegionStructureType, imagePixelSize, GetRegionAbstractRectangle());
        }


        public RectangleF GetRegionInfoRectangleF(Size imagePixelSize)
        {
            return  CalculateImageRegionAbstarctRectangle(imagePixelSize,
                CalculateImageRegionPixelRectangle(this.RegionStructureType, imagePixelSize, this.GetRegionAbstractRectangle()), 
                RegionStructureTypes.MetadataWorkingGroup);
        }

        public RectangleF GetRegionInfoMPRectangleF(Size imagePixelSize)
        {
            return CalculateImageRegionAbstarctRectangle(imagePixelSize,
                CalculateImageRegionPixelRectangle(this.RegionStructureType, imagePixelSize, this.GetRegionAbstractRectangle()),
                RegionStructureTypes.WindowsLivePhotoGallery);
        }

        private static Rectangle CalculateImageRegionPixelRectangle(RegionStructureTypes regionStructureType, Size imagePixelSize, RectangleF regionAreaAbstarctRectangle)
        {
            Rectangle rectangleCopyFrom;

            Size regionPixelSize = CalculateImageRegionPixelSize(regionStructureType, imagePixelSize, regionAreaAbstarctRectangle.Size);

            switch (regionStructureType)
            {
                case RegionStructureTypes.WindowsLivePhotoGallery:
                case RegionStructureTypes.WindowsLivePhotoGalleryDatabase:
                    rectangleCopyFrom = new Rectangle(
                        (int)((float)imagePixelSize.Width * regionAreaAbstarctRectangle.X),
                        (int)((float)imagePixelSize.Height * regionAreaAbstarctRectangle.Y),
                        (int)((float)regionPixelSize.Width), // * regionAreaAbstarctRectangle.Width),
                        (int)((float)regionPixelSize.Height)); // * regionAreaAbstarctRectangle.Height));
                    break;                
                case RegionStructureTypes.MetadataWorkingGroup:
                
                    rectangleCopyFrom = new Rectangle(
                        (int)((float)imagePixelSize.Width * regionAreaAbstarctRectangle.X - regionPixelSize.Width * 0.5),
                        (int)((float)imagePixelSize.Height * regionAreaAbstarctRectangle.Y - regionPixelSize.Height * 0.5),
                        (int)((float)regionPixelSize.Width),
                        (int)((float)regionPixelSize.Height));
                    break;
                case RegionStructureTypes.IptcPixel: //Not testeded and this means not implemented                
                    /*rectangleCopyFrom = new Rectangle(
                        (int)regionAreaAbstarctRectangle.X,
                        (int)regionAreaAbstarctRectangle.Y,
                        (int)regionAreaAbstarctRectangle.Width,
                        (int)regionAreaAbstarctRectangle.Height);*/
                    throw new Exception("Not implemented");
                    //break;
                case RegionStructureTypes.MicrosoftPhotosDatabase:
                    rectangleCopyFrom = new Rectangle(
                        (int)((float)imagePixelSize.Width * regionAreaAbstarctRectangle.X - regionPixelSize.Width * 0.0),
                        (int)((float)imagePixelSize.Height * regionAreaAbstarctRectangle.Y - regionPixelSize.Height),
                        (int)((float)regionPixelSize.Width),
                        (int)((float)regionPixelSize.Height));
                    break;

                default:
                    throw new Exception("Not implemented");
                    //break;
            }

            //In case of wrong size in EXIF data, check size and make sure not copy outside allocateded picture
            if (rectangleCopyFrom.X < 0 || rectangleCopyFrom.X > imagePixelSize.Width) rectangleCopyFrom.X = 0;
            if (rectangleCopyFrom.Y < 0 || rectangleCopyFrom.Y > imagePixelSize.Height) rectangleCopyFrom.Y = 0;
            if (rectangleCopyFrom.X + rectangleCopyFrom.Width > imagePixelSize.Width) 
                rectangleCopyFrom.Width = imagePixelSize.Width - rectangleCopyFrom.X;
            if (rectangleCopyFrom.Y + rectangleCopyFrom.Height > imagePixelSize.Height) 
                rectangleCopyFrom.Height = imagePixelSize.Height - rectangleCopyFrom.Y;

            return rectangleCopyFrom;
        }

        public static RectangleF CalculateImageRegionAbstarctRectangle(Size imagePixelSize, Rectangle regionPixelRectangle, RegionStructureTypes regionStructureType)
        {
            RectangleF rectangleAbstract;

            switch (regionStructureType)
            {
                case RegionStructureTypes.WindowsLivePhotoGallery:
                case RegionStructureTypes.WindowsLivePhotoGalleryDatabase:
                    rectangleAbstract = new RectangleF(
                        (float)regionPixelRectangle.X / (float)imagePixelSize.Width,
                        (float)regionPixelRectangle.Y / (float)imagePixelSize.Height,
                        (float)regionPixelRectangle.Width / (float)imagePixelSize.Width,
                        (float)regionPixelRectangle.Height / (float)imagePixelSize.Height);
                    break;
                case RegionStructureTypes.MetadataWorkingGroup:
                    rectangleAbstract = new RectangleF(
                        (float)(regionPixelRectangle.X + regionPixelRectangle.Width * 0.5 ) / (float)imagePixelSize.Width,
                        (float)(regionPixelRectangle.Y + regionPixelRectangle.Height * 0.5) / (float)imagePixelSize.Height,
                        (float)regionPixelRectangle.Width / (float)imagePixelSize.Width,
                        (float)regionPixelRectangle.Height / (float)imagePixelSize.Height);
                    break;
                case RegionStructureTypes.IptcPixel: //Not testeded and this means not implemented                
                    /*rectangleCopyFrom = new Rectangle(
                        (int)regionAreaAbstarctRectangle.X,
                        (int)regionAreaAbstarctRectangle.Y,
                        (int)regionAreaAbstarctRectangle.Width,
                        (int)regionAreaAbstarctRectangle.Height);*/
                    throw new Exception("Not implemented");
                    //break;
                case RegionStructureTypes.MicrosoftPhotosDatabase:                    
                    rectangleAbstract = new RectangleF(
                        (float)(regionPixelRectangle.X) / (float)imagePixelSize.Width,
                        (float)(regionPixelRectangle.Y + regionPixelRectangle.Height) / (float)imagePixelSize.Height,
                        (float)regionPixelRectangle.Width  / (float)imagePixelSize.Width,
                        (float)regionPixelRectangle.Height / (float)imagePixelSize.Height);
                    break;
                default:               
                    throw new Exception("Not implemented");
                    //break;
            }

            //In case of wrong size in EXIF data, check size and make sure not copy outside allocateded picture
            if (rectangleAbstract.X < 0 || rectangleAbstract.X > imagePixelSize.Width) rectangleAbstract.X = 0;
            if (rectangleAbstract.Y < 0 || rectangleAbstract.Y > imagePixelSize.Height) rectangleAbstract.X = 0;
            if (rectangleAbstract.X + rectangleAbstract.Width > imagePixelSize.Width) 
                rectangleAbstract.Width = imagePixelSize.Width - rectangleAbstract.X;
            if (rectangleAbstract.Y + rectangleAbstract.Height > imagePixelSize.Height) 
                rectangleAbstract.Height = imagePixelSize.Height - rectangleAbstract.Y;

            return rectangleAbstract;
        }

        
    }
}
