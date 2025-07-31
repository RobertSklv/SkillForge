import { Icon } from '@/components/icon/Icon';
import type TagLinkType from '@/lib/types/TagLinkType';
import './_tag-link.scss';
import { Link } from '@/components/link/Link';
import type { BootstrapColor } from '@/lib/types/BootstrapColor';

export interface ITagLinkProps {
    data: TagLinkType;
    size?: 'normal' | 'small';
    background?: 'outline' | 'fill';
    color?: BootstrapColor;
    muted?: boolean;
}

export function TagLink (props: ITagLinkProps) {
  return (
    <Link
        href={`/tag/${props.data.Name}`}
        size={props.size}
        background={props.background}
        color={props.color}
        borderRadius={3}
        classes="tag-link"
        muted={props.muted}
    >
        <div className="d-flex tag-link">
            <Icon type="hash" />
            <span className="text-truncate">{props.data.Name}</span>
        </div>
    </Link>
  );
}
