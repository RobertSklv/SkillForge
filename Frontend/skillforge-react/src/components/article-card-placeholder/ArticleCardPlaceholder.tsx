import * as React from 'react';
import { Block } from '@/components/block/Block';
import { Icon } from '@/components/icon/Icon';
import { AuthorBoxPlaceholder } from '@/components/author-box-placeholder/AuthorBoxPlaceholder';
import { CommentLimitedPlaceholder } from '@/components/comment-limited-placeholder/CommentLimitedPlaceholder';
import { RateButtonsPlaceholder } from '@/components/rate-buttons-placeholder/RateButtonsPlaceholder';
import { LinkPlaceholder } from '@/components/link-placeholder/LinkPlaceholder';
import { TagLinkPlaceholder } from '@/components/tag-link-placeholder/TagLinkPlaceholder';

export function ArticleCardPlaceholder () {
  return (
	<Block classes="placeholder-glow">
		<div className="placeholder card-img-top rounded-top-3 object-fit-cover" style={{ height: '250px' }}></div>
		<AuthorBoxPlaceholder classes="mt-3" />
		<div className="card-body mx-5">
			<div className="d-flex justify-content-between align-items-start">
				<div className="h2 card-title fw-bold w-100">
					<span className="placeholder col-5"></span>
				</div>
			</div>
			<div className="d-flex justify-content-between">
				<div className="col">
					<div className="tags d-flex gap-1">
						<TagLinkPlaceholder size="small" background="fill"/>
					</div>
				</div>
				<div>
					<div className="d-flex align-items-center">
						<RateButtonsPlaceholder size="small" />
						<div className="text-primary d-flex align-items-center px-2">
							<Icon type="chat" classes="fs-6" />
							<span className="placeholder placeholder-sm col-3 ms-1 h-auto"></span>
						</div>
					</div>
				</div>
			</div>
		</div>
		<div>
			<CommentLimitedPlaceholder />
		</div>
		<div className="text-center mt-2 mb-4">
			<LinkPlaceholder background="fill" />
		</div>
	</Block>
  );
}
